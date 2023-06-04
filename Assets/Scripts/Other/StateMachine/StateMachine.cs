using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public State CurrentState { get; private set; }


    public void SetState(State state)
    {
        if (state == CurrentState) return;

        CurrentState = state;
    }

    protected virtual void Update()
    {
        if (CurrentState == null) return;

        CurrentState.PerformState();
    }

    protected virtual void FixedUpdate()
    {
        if (CurrentState == null) return;

    }
}
