using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyBase : StateMachine, IDamageable
{
    [Header("Enemy Health")]
    [SerializeField] protected int _maxHealth = 1;
    [SerializeField] protected Image _healthBar = null;

    [Header("Enemy Movement")]
    [SerializeField] protected float _moveSpeed = 2f;
    
    protected Health _health;
    protected bool _isAttacking = false;

    private int _instanceID = 0;
    private bool _isFacingRight = true;

    public GameObject GameObject { get { return gameObject; } }
    public int InstanceID { get { return _instanceID; } }


    public void TakeDamage(int value)
    {
        _health.CurrentHealth -= value;
        Debug.Log(InstanceID + " has been hit");

        if (_health.CurrentHealth <= 0) gameObject.SetActive(false);
    }

    public bool IsTargetReached(Transform target, float targetDistance = 1)
    {
        float distanceFromTarget = Vector2.Distance(transform.position, target.position);
        distanceFromTarget = Calculate.RoundedAbsoluteValue(distanceFromTarget);
        targetDistance = Calculate.RoundedAbsoluteValue(targetDistance);

        //Debug.Log(distanceFromTarget);
        return distanceFromTarget <= targetDistance;
    }

    public virtual void MoveToTargetDirection(Transform target)
    {
        bool isTargetRight = target.position.x > transform.position.x;
        if (isTargetRight && !_isFacingRight)
            Flip();
        if (!isTargetRight && _isFacingRight)
            Flip();
    }

    protected virtual void Flip()
    {
        _isFacingRight = !_isFacingRight;
        _moveSpeed *= -1;
    }

    }

    protected virtual void Awake()
    {
        _rb2D = GetComponent<Rigidbody2D>();

        _health = new Health(_maxHealth, _healthBar);
        _instanceID = gameObject.GetInstanceID();

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        CheckForPlayerCollision();
    }

    private void CheckForPlayerCollision()
    {
        LayerMask player = LayerMask.GetMask("Player");
        if (Physics2D.OverlapBox(transform.position, transform.localScale, 0f, player))
            Debug.Log("Player Hit");
    }
}
