using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CharacterController2D))]
public abstract class GroundEnemy : EnemyBase
{
    [SerializeField] private float _fallingThreshold = -5f;

    [Header("Platform Checks")]
    [SerializeField] protected Transform _wallCheck;
    [SerializeField] protected Transform _groundPlatformCheck;
    [SerializeField] protected LayerMask _whatIsPlatform;

    protected Vector2 _ceilingBoxCastSize = new Vector2(1.5f, 1f);
    protected Vector2 _localScale;

    private CharacterController2D _controller;

    private bool _canJump = false;
    private bool _isGrounded = true;

    protected Transform CurrentTarget { get; private set; }


    public void OnLanding()
    {
        _isGrounded = true;
    }

    public override void MoveToTargetDirection(Transform target)
    {
        CurrentTarget = target;

        if (JumpCondition() && _isGrounded)
            Jump();

        if (IsTargetOnPlatform(target) || IsTargetBelowPlatform(target)) return;

        bool isFalling = _rb2D.velocity.y < _fallingThreshold;
        if (isFalling || _isGrounded)
            base.MoveToTargetDirection(target);
    }

    protected abstract bool JumpCondition();
    protected abstract bool MoveCondition();

    protected override void Awake()
    {
        base.Awake();

        _localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        _controller = GetComponent<CharacterController2D>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(MoveCondition())
            _controller.Move(_moveSpeed * Time.fixedDeltaTime, false, _canJump);
        else
            _controller.Move(0, false, _canJump);

        _canJump = false;
    }

    private void Jump()
    {
        _canJump = true;
        _isGrounded = false;
    }

    private bool IsTargetOnPlatform(Transform target)
    {
        //Assume the target is higher than self based on position
        bool isTargetAbove = target.position.y > transform.position.y;
        if (isTargetAbove)
        {
            //Assume the target is on a platform if self is under a platform
            bool isBelowPlatform = Physics2D.BoxCast(transform.position, _localScale, 0, transform.up, Mathf.Infinity, _whatIsPlatform);
            if (isBelowPlatform) return true;
        }

        return false;
    }

    private bool IsTargetBelowPlatform(Transform target)
    {
        //Assume the target is lower than self based on position
        bool isTargetBelow = target.position.y < transform.position.y;
        if (isTargetBelow)
        {
            //Assume the target is below a platform if self is on a platform
            bool isOnPlatform = Physics2D.BoxCast(_groundPlatformCheck.position, _localScale, 0, _groundPlatformCheck.position, Mathf.Infinity, _whatIsPlatform);
            if (isOnPlatform) return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Vector2 _platformCheck = new Vector2(_wallCheck.position.x, _wallCheck.position.y + _localScale.y + 0.5f);
        Gizmos.DrawCube(_platformCheck, _localScale);

        Gizmos.DrawCube(_wallCheck.position, _localScale);
        Gizmos.DrawCube(_ceilingCheck.position, _ceilingBoxCastSize);
        //Gizmos.DrawCube(_groundPlatformCheck.position, _localScale);
    }
}
