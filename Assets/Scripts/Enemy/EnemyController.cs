using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] protected int health = 1;

    public bool IsHit { get; set; }

    public virtual void TakeDamage(int value)
    {
        health -= value;
        IsHit = true;
        Debug.Log(gameObject.name + " has been hit");

        if (health <= 0) gameObject.SetActive(false);
    }
}
