using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] protected int health = 1;
    [SerializeField] protected float moveSpeed = 2f;

    public bool IsHit { get; set; }

    public virtual void TakeDamage(int value)
    {
        health -= value;
        IsHit = true;
        Debug.Log(gameObject.name + " has been hit");

        if (health <= 0) gameObject.SetActive(false);
    }
    protected virtual void Awake()
    {
        InitializeColliders();       
    }

    private void InitializeColliders()
    {
        //Make sure that enemy colliders are always set to triggers at the start of the game
        //this allows the player to pass through enemies and prevents objects from getting pushed around
        Collider2D ceilingCollider = GetComponent<BoxCollider2D>();
        ceilingCollider.isTrigger = true;

        Collider2D groundCollider = GetComponent<CircleCollider2D>();
        groundCollider.isTrigger = true;
    }
}
