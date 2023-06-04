using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : State
{
    private Transform player;

    public override void PerformState()
    {
        base.PerformState();

        _enemyBase.MoveToTargetDirection(player);
    }

    private void Start()
    {
        player = GameController.Instance.Player.transform;
    }
}
