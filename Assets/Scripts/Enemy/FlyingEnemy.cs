using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : EnemyBase
{
    [SerializeField] private float _hoverSpeed = 1f;
    [SerializeField] private float _hoverDistance = 1f;
    [Range(0, .3f)] [SerializeField] private float _movementSmoothing = 0.05f;

    private Rigidbody2D _rb2D;
    private Vector3 _velocity = Vector3.zero;

    private float _currentHoverDistance;
    private bool _isHoveringUp = true;


    public override void MoveToTargetDirection(Transform target)
    {
        /*if (IsTargetReached(target))
        {
            _rb2D.velocity = new Vector2(0, 0);
            return;
        }*/

        base.MoveToTargetDirection(target);

        //allows for more free movement
        _moveSpeed = Mathf.Abs(_moveSpeed) * -1; //_moveSpeed value must always be negative
        float move = (_moveSpeed * Time.fixedDeltaTime) * 3f;
        Vector2 direction = (transform.position - target.position).normalized;
        Vector3 targetVelocity = direction * move;
        _rb2D.velocity = Vector3.SmoothDamp(_rb2D.velocity, targetVelocity, ref _velocity, _movementSmoothing);
    }

    protected override void Awake()
    {
        base.Awake();

        //initialize colliders
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
            collider.isTrigger = true;

        _rb2D = GetComponent<Rigidbody2D>();
    }

    protected override void Flip()
    {
        base.Flip();
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(!_isAttacking)
            Hover();
    }

    private void Hover()
    {       
        float hover = _hoverSpeed * Time.fixedDeltaTime;
        _currentHoverDistance += hover;
        if (_currentHoverDistance >= _hoverDistance && _isHoveringUp)
            FlipHover();
        else if(_currentHoverDistance <= _hoverDistance && !_isHoveringUp)
            FlipHover();

        Vector3 targetVelocity = new Vector2(_rb2D.velocity.x, hover);
        _rb2D.velocity = Vector3.SmoothDamp(_rb2D.velocity, targetVelocity, ref _velocity, _movementSmoothing);
    }

    private void FlipHover()
    {
        _isHoveringUp = !_isHoveringUp;
        _hoverDistance *= -1;
        _hoverSpeed *= -1;
    }
}
