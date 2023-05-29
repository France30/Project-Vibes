using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CharacterController2D))]
public class GroundEnemy : EnemyBase
{
    [SerializeField] private GroundEnemyType _groundEnemyType;
    [SerializeField] private Transform _ceilingCheck;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private LayerMask _whatIsPlatform;
    [SerializeField] private float _fallingThreshold = -5f;

    private CharacterController2D _controller;
    private Rigidbody2D _rb2D; 

    private Vector2 _ceilingBoxCastSize = new Vector2(1.5f, 1f);
    private Vector2 _wallBoxCastSize;

    private bool _canJump = false;
    private bool _isGrounded = true;

    public delegate void OnEnemyJump();
    public event OnEnemyJump OnEnemyJumpEvent;

    public void OnLanding()
    {
        _isGrounded = true;
    }

    protected override void Awake()
    {
        base.Awake();
        _controller = GetComponent<CharacterController2D>();
        _rb2D = GetComponent<Rigidbody2D>();

        _groundEnemyType.FallingThreshold = _fallingThreshold;

        _groundEnemyType.InitializeEnemy(this);
        _groundEnemyType.InitializeEnemyPosition(transform);
        _groundEnemyType.InitializeRigidbody2D(_rb2D);

        _wallBoxCastSize = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        _groundEnemyType.InitializeChecks(_wallCheck, _ceilingCheck, _wallBoxCastSize, _ceilingBoxCastSize, _whatIsPlatform);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(_groundEnemyType.MoveCondition())
            _controller.Move(_moveSpeed * Time.fixedDeltaTime, false, _canJump);
        else
            _controller.Move(0, false, _canJump);

        _canJump = false;
    }

    protected override void MoveToTargetDirection(Transform target)
    {
        _groundEnemyType.CurrentTarget = target;

        if (_groundEnemyType.JumpCondition())
            Jump();

        //Do not update direction if target is on a platform and enemy is underneath the same platform
        //This is so the enemy doesn't get stuck since ground enemies only follow the x position of the target
        bool isTargetAbove = target.position.y > transform.position.y;
        if (isTargetAbove)
        {
            bool isBelowPlatform = Physics2D.BoxCast(_ceilingCheck.position, _ceilingBoxCastSize, 0, transform.up, Mathf.Infinity, _whatIsPlatform);
            if (isBelowPlatform) return;
        }

        bool isFalling = _rb2D.velocity.y < _fallingThreshold;
        if (isFalling || _isGrounded)
            base.MoveToTargetDirection(target);
    }

    private void Jump()
    {
        _canJump = true;
        _isGrounded = false;

        OnEnemyJumpEvent?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(_wallCheck.position, _wallBoxCastSize);
        Gizmos.DrawCube(_ceilingCheck.position, _ceilingBoxCastSize);
    }

    private void OnEnable()
    {
        _groundEnemyType.RegisterEvents();
    }

    private void OnDisable()
    {
        _groundEnemyType.UnregisterEvents();
    }

    {

    }
}
