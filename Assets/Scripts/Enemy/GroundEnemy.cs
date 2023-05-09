using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : EnemyController
{
    private CharacterController2D controller;
    
    private void Awake()
    {
        controller = GetComponent<CharacterController2D>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        controller.Move(moveSpeed * Time.fixedDeltaTime, false, false);
    }
}
