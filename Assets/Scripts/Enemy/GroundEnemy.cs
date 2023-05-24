using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CharacterController2D))]
public class GroundEnemy : EnemyBase
{
    [SerializeField] private Transform[] _wayPoints;
    [SerializeField] private Transform _ceilingCheck;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private float _fallingThreshold = -5f;

    private CharacterController2D _controller;
    private Rigidbody2D _rb2D;

    private Vector2 _wallBoxCastSize = new Vector2(.5f, 1.5f);
    private Vector2 _ceilingBoxCastSize = new Vector2(1.5f, 1f);

    private int _currentWayPoint = 0;
    private bool _canJump = false;
    private bool _isGrounded = true;


    public void OnLanding()
    {
        _isGrounded = true;
    }

    protected override void Awake()
    {
        base.Awake();
        _controller = GetComponent<CharacterController2D>();
        _rb2D = GetComponent<Rigidbody2D>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Patrol();

        _controller.Move(_moveSpeed * Time.fixedDeltaTime, false, _canJump);
        _canJump = false;
    }

    protected override void MoveToTargetDirection(Transform target)
    {
        bool isTargetAbove = target.position.y > transform.position.y;

        //jump if platform is found
        bool isTherePlatform = Physics2D.OverlapBox(_wallCheck.position, _wallBoxCastSize, 0, _whatIsGround);
        if (isTherePlatform && isTargetAbove)
        {
            _canJump = true;
            _isGrounded = false;
        }
            

        //Do not update direction if target is on a platform and enemy object is underneath said platform
        //Use BoxCast to constantly "sweep"
        bool isBelowCeiling = Physics2D.BoxCast(_ceilingCheck.position, _ceilingBoxCastSize, 0, _ceilingCheck.position, Mathf.Infinity, _whatIsGround);

        if (isTargetAbove && isBelowCeiling) return;

        bool isFalling = _rb2D.velocity.y < _fallingThreshold;
        if (isFalling || _isGrounded)
            base.MoveToTargetDirection(target);
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
