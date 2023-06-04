using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public abstract class State : MonoBehaviour
{
    protected EnemyBase _enemyBase;
    
    public delegate void Action();
    protected Action StateAction;   //Callback delegate for state specific actions
    protected Action StatePhysics;  //Callback delegate for states containing physics (if any)


    public virtual void PerformState()
    {
        if (StateAction != null)
            StateAction();
    }

    public virtual void PerformPhysics()
    {
        if (StatePhysics != null)
            StatePhysics();
    }

    public void SetAction(Action stateAction)
    {
        if (StateAction != null) return;

        StateAction = stateAction;
    }

    public void SetPhysics(Action statePhysics)
    {
        if (StatePhysics != null) return;

        StatePhysics = statePhysics;
    }

    private void Awake()
    {
        _enemyBase = GetComponent<EnemyBase>();
    }
}
