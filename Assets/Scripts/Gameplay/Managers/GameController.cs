using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [Header("Game Over Settings")]
    [SerializeField] private string _gameOverText = "You Died";
    [SerializeField] private float _freezeDeathEffectDuration = 1f;
    [SerializeField] private float _timeTillLevelReset = 2f;

    private Player _player;
    private bool _isPaused = false;
    private bool _isGameOver = false;

    public delegate void OnPause(bool isPaused);
    public event OnPause OnPauseEvent;

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


    protected override void Awake()
    {
        base.Awake();
 
        GameUIManager intializeGameUI = GameUIManager.Instance; //for easier testing, will remove at a later time

        _player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        _player.OnPlayerDeath += GameOver;
        LevelManager.Instance.OnLevelLoad += DisableGame;
    }

    private void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnPlayerDeath -= GameOver;
        }

        if(LevelManager.Instance != null)
        {
            LevelManager.Instance.OnLevelLoad -= DisableGame;
        }
    }

    private void Update()
    {
        if (_isGameOver) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
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
            GameUIManager.Instance.TextNotif.text = _gameOverText;
            StartCoroutine(GameOverSequence());
        }
    }

    private IEnumerator GameOverSequence()
    {
        yield return StartCoroutine(FreezeDeathEffect());
        yield return new WaitUntil(IsGameOverNotifDone);
        yield return new WaitForSeconds(_timeTillLevelReset);

        DisableGame();
        LevelManager.Instance.ResetLevel();
    }

    private bool IsGameOverNotifDone()
    {
        bool isNotifDone = GameUIManager.Instance.TextNotif.alpha >= 1f;
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
}
