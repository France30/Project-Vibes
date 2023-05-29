using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    [SerializeField] protected int _maxHealth = 1;
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

    protected virtual void Awake()
    {
        _health = new Health(_maxHealth);
    }

    protected bool IsTargetReached(float distanceFromTarget, float targetDistance = 0)
    {
        targetDistance = Calculate.RoundedAbsoluteValue(targetDistance);
        distanceFromTarget = Calculate.RoundedAbsoluteValue(distanceFromTarget);

        //Debug.Log(targetDistance);
        return distanceFromTarget <= targetDistance;
    }

    protected virtual void MoveToTargetDirection(Transform target)
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

    protected virtual void FixedUpdate()
    {
        CheckForPlayerCollision();
    }

    private void CheckForPlayerCollision()
    {
        LayerMask player = LayerMask.GetMask("Player");
        if (Physics2D.OverlapBox(transform.position, transform.localScale, 0f, player))
            Debug.Log("Player Hit");
    }
}
