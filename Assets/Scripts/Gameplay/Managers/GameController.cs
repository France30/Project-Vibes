using System.Collections;
using UnityEngine;

public class GameController : Singleton<GameController>
{
	[Header("Game Over Settings")]
	[SerializeField] private string _gameOverText = "You Died";
	[SerializeField] private float _freezeDeathEffectDuration = 1f;
	[SerializeField] private float _timeTillLevelReset = 2f;

	private Player _player;
	private BossEnemy _boss;
	private BeatSystemController _beatSystem;
	private GameUIManager _gameUI;
	private bool _isPaused = false;
	private bool _isGameOver = false;
	private bool _isPrologueEnd = false;
	private bool _isGameControlsDisabled = false;

	public delegate void OnGameEvent(bool isGameEvent);
	public event OnGameEvent OnPauseEvent;
	public event OnGameEvent OnPrologueEnd;
	public event OnGameEvent OnFreezeEffect;
	public event OnGameEvent OnDisableGameControls;

	public Player Player { 
		get 
		{
			if (_player == null)
				_player = FindObjectOfType<Player>();

			return _player;
		} 
	}

	public BossEnemy Boss
	{
		get
		{
			if (_boss == null)
				_boss = FindObjectOfType<BossEnemy>();

			return _boss;
		}
	}

	public void DisableGameControls(bool isDisable)
    {
		_beatSystem.gameObject.SetActive(!isDisable);
		_gameUI.gameObject.SetActive(!isDisable);

		OnDisableGameControls?.Invoke(isDisable);
		_isGameControlsDisabled = isDisable;
    }

	public void ResetGameControllerConfigs()
    {
		_isGameOver = false;
		_isPrologueEnd = false;

		if (_isPaused)
			TogglePause();
    }

	protected override void Awake()
	{
		base.Awake();
 
		//cache reference to game systems
		_gameUI = GameUIManager.Instance;
		_beatSystem = BeatSystemController.Instance;

		_player = FindObjectOfType<Player>();
		_boss = FindObjectOfType<BossEnemy>();
	}

	private void OnEnable()
	{
		OnPrologueEnd += SetPrologueEnd;
		_player.OnPlayerDeath += GameOver;
		if(_boss != null)
			_boss.EnemyDeathSequence.OnAnimationStart += StartPrologueEnd;

		LevelManager.Instance.OnLevelLoad += DisableGame;
	}

	private void OnDestroy()
	{
		OnPrologueEnd -= SetPrologueEnd;

		if (_player != null)
		{
			_player.OnPlayerDeath -= GameOver;
		}

		if (_boss != null)
		{
			_boss.EnemyDeathSequence.OnAnimationStart -= StartPrologueEnd;
		}

		if(LevelManager.Instance != null)
		{
			LevelManager.Instance.OnLevelLoad -= DisableGame;
		}
	}

	private void Update()
	{
		if (_isGameOver || _isPrologueEnd || _isGameControlsDisabled) return;

		if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
		{
			TogglePause();
		}
	}

	private void StartPrologueEnd()
	{
		OnPrologueEnd?.Invoke(true);
		SaveSystem.ClearAllSaveData();
	}

	//Use this method to disable any dependencies first
	private void DisableGame()
	{
		EnemyBase[] enemy = FindObjectsOfType<EnemyBase>();
		Utilities.DisableAllInstancesOfType<EnemyBase>(enemy);

		GameUIManager.Instance.enabled = false;
		_player.enabled = false;
	}

	private void TogglePause()
	{
		_isPaused = !_isPaused;
		Time.timeScale = (_isPaused) ? 0 : 1;
		//Debug.Log("is Paused: " + _isPaused);
		//Debug.Log("is Game Over: " + _isGameOver);

		if (!_isGameOver)
			OnPauseEvent?.Invoke(_isPaused);
	}

	private void GameOver(bool isGameOver)
	{
		_isGameOver = isGameOver;

		if (_isGameOver)
		{
			GameUIManager.Instance.SetTextNotifAlpha(0);
			GameUIManager.Instance.Notification.text = _gameOverText;
			StartCoroutine(GameOverSequence());
		}
	}

	private IEnumerator GameOverSequence()
	{
		yield return StartCoroutine(FreezeDeathEffect());
		yield return new WaitUntil(IsGameOverNotificationDone);
		yield return new WaitForSeconds(_timeTillLevelReset);

		DisableGame();
		PlayerData playerData = SaveSystem.LoadPlayerData();
		if(playerData != null)
		{
			LevelManager.Instance.LoadLevelFromSave(false);
		}
		else
		{
			LevelManager.Instance.ResetLevel(false);
		}
	}

	private bool IsGameOverNotificationDone()
	{
		bool isNotifDone = GameUIManager.Instance.Notification.alpha >= 1f;
		if(!isNotifDone)
		{
			GameUIManager.Instance.FadeInNotificationText();
		}
		return isNotifDone;
	}

	private IEnumerator FreezeDeathEffect()
	{
		OnFreezeEffect?.Invoke(true);
		TogglePause();

		yield return new WaitForSecondsRealtime(_freezeDeathEffectDuration);

		TogglePause();
		OnFreezeEffect?.Invoke(false);
	}

	private void SetPrologueEnd(bool isEnd)
	{
		_isPrologueEnd = isEnd;
	}
}
