using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [Header("Game Over Settings")]
    [SerializeField] private float _freezeDeathEffectDuration = 1f;
    [SerializeField] private float _timeTillLevelReset = 2f;

    private Player _player;
    private bool _isPaused = false;
    private bool _isGameOver = false;

    public delegate void OnPause(bool isPaused);
    public event OnPause OnPauseEvent;

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
        _player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        _player.OnPlayerDeath += GameOver;
    }

    private void OnDestroy()
    {
        if (_player == null) return;

        _player.OnPlayerDeath -= GameOver;
    }

    private void Update()
    {
        if (_isGameOver) return;

        if (Input.GetKeyDown(KeyCode.P))
            TogglePause();
    }

    //Use this method to disable any dependencies first
    private void DisableGame()
    {
        _player.enabled = false;

        EnemyBase[] enemy = FindObjectsOfType<EnemyBase>();
        for (int i = 0; i < enemy.Length; i++)
        {
            if (!enemy[i].enabled) continue;

            enemy[i].enabled = false;
        }
    }

    private void TogglePause()
    {
        _isPaused = !_isPaused;
        Time.timeScale = (_isPaused) ? 0 : 1;
        OnPauseEvent?.Invoke(_isPaused);
    }

    private void GameOver(bool isGameOver)
    {
        if (isGameOver)
            StartCoroutine(GameOverSequence());

        _isGameOver = isGameOver;
    }

    private IEnumerator GameOverSequence()
    {
        yield return StartCoroutine(FreezeDeathEffect());

        _player.GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(_timeTillLevelReset);

        DisableGame();
        LevelManager.Instance.ResetLevel();
    }

    private IEnumerator FreezeDeathEffect()
    {
        TogglePause();

        yield return new WaitForSecondsRealtime(_freezeDeathEffectDuration);

        TogglePause();
    }
}
