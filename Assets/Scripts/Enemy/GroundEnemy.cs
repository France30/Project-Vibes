using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class GroundEnemy : EnemyController
{
    private CharacterController2D _controller;


    protected override void Awake()
    {
        base.Awake();
        _controller = GetComponent<CharacterController2D>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        _controller.Move(_moveSpeed * Time.fixedDeltaTime, false, false);
    }
}
