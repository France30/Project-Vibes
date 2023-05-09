using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : EnemyController
{

    private CharacterController2D controller;

    protected override void Awake()
    {
        base.Awake();
        controller = GetComponent<CharacterController2D>();

    }

    private void FixedUpdate()
    {
        controller.Move(moveSpeed * Time.fixedDeltaTime, false, false);
    }

}
