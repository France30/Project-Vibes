using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{


    private Player _player;
    public Player Player { 
        get 
        {
            if (_player == null)
                _player = FindObjectOfType<Player>();

            return _player;
        } 
    }

    private void TogglePause()
    {
    }
}
