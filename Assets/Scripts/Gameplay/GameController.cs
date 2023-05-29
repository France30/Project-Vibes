using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public Player Player { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        Player = FindObjectOfType<Player>();
    }
}
