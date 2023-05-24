using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CharacterController2D))]
public class GroundEnemy : EnemyBase
{
    private CharacterController2D _controller;
    private Rigidbody2D _rb2D;


    protected override void Awake()
    {
        base.Awake();
        _controller = GetComponent<CharacterController2D>();
        _rb2D = GetComponent<Rigidbody2D>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        _controller.Move(_moveSpeed * Time.fixedDeltaTime, false, false);
    }
}
