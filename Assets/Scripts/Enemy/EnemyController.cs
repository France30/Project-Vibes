using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] protected int maxHealth = 1;
    [SerializeField] protected float moveSpeed = 2f;

    protected bool isAttacking = false;

    private bool isFacingRight = true;
    
    public bool IsHit { get; set; }


    public virtual void TakeDamage(int value)
    {
        maxHealth -= value;
        IsHit = true;
        Debug.Log(gameObject.name + " has been hit");

        if (maxHealth <= 0) gameObject.SetActive(false);
    }

    protected virtual void MoveToTargetDirection(Transform target)
    {
        bool isTargetRight = target.position.x > transform.position.x;
        if (isTargetRight && !isFacingRight)
            Flip();
        if (!isTargetRight && isFacingRight)
            Flip();
    }

    protected virtual void Flip()
    {
        isFacingRight = !isFacingRight;
        moveSpeed *= -1;
    }

    protected virtual void FixedUpdate()
    {
        CheckForPlayerCollision();
    }

    private void CheckForPlayerCollision()
    {
        LayerMask player = LayerMask.GetMask("Player");
        if (Physics2D.BoxCast(transform.position, transform.localScale, 0f, Vector2.zero, 0f, player))
            Debug.Log("Player Hit");
    }
}
