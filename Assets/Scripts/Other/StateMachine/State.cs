using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public abstract class State : MonoBehaviour
{
    protected EnemyBase _enemyBase;
    
    public delegate void Action();
    protected Action StateAction;   //Callback delegate for state specific actions


    public virtual void PerformState()
    {
        if (StateAction != null)
            StateAction();
    }

    }

    public void SetAction(Action stateAction)
    {
        if (StateAction != null) return;

        StateAction = stateAction;
    }

    private void Awake()
    {
        _enemyBase = GetComponent<EnemyBase>();
    }
}
