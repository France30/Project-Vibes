using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CharacterController2D))]
public class GroundEnemy : EnemyBase
{
    [SerializeField] private Transform[] _wayPoints;

    private CharacterController2D _controller;
    private Rigidbody2D _rb2D;

    private int _currentWayPoint = 0;

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

        Patrol();
    }

    private void Patrol()
    {
        Transform currentPatrol = _wayPoints[_currentWayPoint];
        float distanceFromTarget = transform.position.x - currentPatrol.position.x;
        bool isTargetAbove = currentPatrol.position.y > transform.position.y;
        if (IsTargetReached(distanceFromTarget) && !isTargetAbove)
            GoToNextWayPoint();

        MoveToTargetDirection(currentPatrol);   
    }

    private void GoToNextWayPoint()
    {
        _currentWayPoint++;

        if (_currentWayPoint > _wayPoints.Length - 1)
            _currentWayPoint = 0;
    }
}
