using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CharacterController2D))]
public abstract class GroundEnemy : EnemyBase
{
    [SerializeField] private float _fallingThreshold = -5f;

    [Header("Platform Checks")]
    [SerializeField] protected Transform _wallCheck;
    [SerializeField] protected Transform _groundPlatformCheck;
    [SerializeField] protected LayerMask _whatIsPlatform;

    protected RaycastHit2D[] _hitDetect = new RaycastHit2D[100];
    protected Collider2D[] _overlapDetect = new Collider2D[100];

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

        if (IsTargetOnPlatform(target) || IsTargetBelowPlatform(target))
        {
            if (!IsCollidingWithOtherGroundEnemy()) return;
        }

        bool isFalling = _rb2D.velocity.y < _fallingThreshold;
        if (isFalling || _isGrounded)
            base.MoveToTargetDirection(target);
    }

    protected abstract bool JumpCondition();
    protected abstract bool MoveCondition();

    private void Start()
    {
        _controller = GetComponent<CharacterController2D>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(MoveCondition())
            _controller.Move(_moveSpeed * Time.fixedDeltaTime, false, _canJump);
        else
            _controller.Move(0, false, false);

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
            int colliders = Physics2D.BoxCastNonAlloc(transform.position, _spriteSize, 0, transform.up, _hitDetect, Mathf.Infinity, _whatIsPlatform);
            if (colliders > 0) return true;
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
            int colliders = Physics2D.BoxCastNonAlloc(_groundPlatformCheck.position, _spriteSize, 0, -transform.up, _hitDetect, _groundPlatformCheck.position.y, _whatIsPlatform);
            if (colliders > 0) return true;
        }

        return false;
    }

    private bool IsCollidingWithOtherGroundEnemy()
    {
        LayerMask enemyLayer = gameObject.layer;
        int colliders = Physics2D.BoxCastNonAlloc(transform.position, _spriteSize, 0, transform.position, _hitDetect, enemyLayer);
        if (colliders > 0) return true;

        return false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireCube(_wallCheck.position, _spriteSize);
        Gizmos.DrawWireCube(_groundPlatformCheck.position, _spriteSize);
    }
}
