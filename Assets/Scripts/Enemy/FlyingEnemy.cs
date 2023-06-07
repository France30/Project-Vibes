using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : EnemyBase
{
    [Range(0, .3f)] [SerializeField] private float _movementSmoothing = 0.05f;

    [Header("Hover")]
    [SerializeField] private float _hoverSpeed = 1f;
    [SerializeField] private float _hoverDistance = 1f;

    [Header("Attack Configs")]
    [SerializeField] private FlyingAttackConfigs _flyingAttackConfigs;

    private Vector3 _velocity = Vector3.zero;
    private Vector3 _targetVelocity = Vector3.zero;

    private float _currentHoverDistance;
    private bool _isHoveringUp = true;


    public override void MoveToTargetDirection(Transform target)
    {
        base.MoveToTargetDirection(target);

        //Disregard movement if attacking
        if (_isAttacking) return;

        _moveSpeed = Mathf.Abs(_moveSpeed) * -1; //moveSpeed value must always be negative
        float move = (_moveSpeed * Time.fixedDeltaTime) * 3f;
        Vector3 direction = (transform.position - target.position).normalized;
        _targetVelocity = direction * move;
    }

    protected override void Awake()
    {
        base.Awake();

        //initialize colliders
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
            collider.isTrigger = true;

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        _rb2D.velocity = Vector3.SmoothDamp(_rb2D.velocity, _targetVelocity, ref _velocity, _movementSmoothing);
        
        _targetVelocity = Vector3.zero;
        _isAttacking = false;
    }

    protected override void Flip()
    {
        base.Flip();
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

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
}
