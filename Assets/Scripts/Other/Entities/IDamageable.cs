using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public int InstanceID { get; }

    public void TakeDamage(int value);
}
