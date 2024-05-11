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
	private bool _isPaused = false;
	private bool _isGameOver = false;
	private bool _isPrologueEnd = false;

	public delegate void OnPause(bool isPaused);
	public event OnPause OnPauseEvent;

	public delegate void PrologueEnd(bool isEnd);
	public event PrologueEnd OnPrologueEnd;

	public delegate void FreezeDeathEvent(bool isFreezeEvent);
	public event FreezeDeathEvent OnFreezeEffect;

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


	protected override void Awake()
	{
		base.Awake();
 
		GameUIManager intializeGameUI = GameUIManager.Instance; //for easier testing, will remove at a later time

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
		if (_isGameOver || _isPrologueEnd) return;

		if (Input.GetKeyDown(KeyCode.P))
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
			LevelManager.Instance.LoadLevelFromSave();
		}
		else
		{
			LevelManager.Instance.ResetLevel();
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
