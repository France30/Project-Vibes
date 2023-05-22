using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public GameObject GameObject { get; }
    public bool IsHit { get; set; }

    public void TakeDamage(int value);
}
