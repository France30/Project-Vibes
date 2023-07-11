using UnityEngine;

public class FlyingEnemy : EnemyBase
{
    [Range(0, .3f)] [SerializeField] private float _movementSmoothing = 0.05f;

    [Header("Hover")]
    [SerializeField] private float _hoverSpeed = 1f;
    [SerializeField] private float _hoverDistance = 1f;

    [Header("Attack Configs")]
    [SerializeField] private ScriptableObject _ability;

    private Vector3 _velocity = Vector3.zero;
    private Vector3 _targetVelocity = Vector3.zero;
    private Quaternion _targetRotation = Quaternion.identity;

    private float _currentHoverDistance;
    private bool _isHoveringUp = true;

    private IFlyingAttack FlyingAttack { get { return _ability as IFlyingAttack; } }


    public override void MoveToTargetDirection(Transform target)
    {
        base.MoveToTargetDirection(target);
        _targetRotation = EnemyUtilities.LookAtTarget(transform, target);

        //Disregard movement if attacking
        if (IsAttacking) return;

        _targetVelocity = EnemyUtilities.FreeMoveTowardsTarget(ref _moveSpeed, transform, target);
    }

    private void OnValidate()
    {
        if (!gameObject.activeInHierarchy) return;

        if(_ability is not IFlyingAttack)
        {
            throw new System.NullReferenceException("Ability Must Contain Type Of 'IFlyingAttack'");
        }
    }

    private void Start()
    {
        if (TryGetComponent<Idle>(out Idle idle))
            idle.SetAction(Hover);

        SetAttack(() => { _targetVelocity = FlyingAttack.ApplyAttackVelocity(_moveSpeed, transform); });
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        _rb2D.velocity = Vector3.SmoothDamp(_rb2D.velocity, _targetVelocity, ref _velocity, _movementSmoothing);
        transform.rotation = _targetRotation;

        if (IsIdle) LookAhead();
        _targetVelocity = Vector3.zero;
    }

    protected override void Flip()
    {
        base.Flip();
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        theScale.y *= -1;
        transform.localScale = theScale;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private void Hover()
    {
        float hover = _hoverSpeed * Time.fixedDeltaTime;
        _currentHoverDistance += hover;
        if (_currentHoverDistance >= _hoverDistance && _isHoveringUp)
            FlipHover();
        else if(_currentHoverDistance <= _hoverDistance && !_isHoveringUp)
            FlipHover();

        _targetVelocity = new Vector2(0, hover);
    }

    private void FlipHover()
    {
        _isHoveringUp = !_isHoveringUp;
        _hoverDistance *= -1;
        _hoverSpeed *= -1;
    }

    private void LookAhead()
    {
        if (transform.localScale.x > 0)
            _targetRotation = Quaternion.identity;
        if (transform.localScale.x < 0)
            _targetRotation = Quaternion.Euler(0, 0, 180);
    }
}
