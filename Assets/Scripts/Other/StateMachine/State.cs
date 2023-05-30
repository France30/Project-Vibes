using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public abstract class State : MonoBehaviour
{
    protected EnemyBase _enemyBase;


    public abstract void PerformState();

    private void Awake()
    {
        _enemyBase = GetComponent<EnemyBase>();
    }
}
