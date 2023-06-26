using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    private bool _isPaused = false;

    public delegate void OnPause(bool isPaused);
    public event OnPause OnPauseEvent;


    private Player _player;
    public Player Player { 
        get 
        {
            if (_player == null)
                _player = FindObjectOfType<Player>();

            return _player;
        } 
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
            TogglePause();
    }

    private void TogglePause()
    {
        _isPaused = !_isPaused;
        Time.timeScale = (_isPaused) ? 0 : 1;
        OnPauseEvent?.Invoke(_isPaused);
    }

    private IEnumerator GameOverSequence()
    {
    }
}
