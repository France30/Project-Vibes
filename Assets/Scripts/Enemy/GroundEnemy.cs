using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class GroundEnemy : EnemyController
{
    private CharacterController2D controller;
 

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        controller.Move(moveSpeed * Time.fixedDeltaTime, false, false);
    }

    private void Awake()
    {
        controller = GetComponent<CharacterController2D>();
    }
}
