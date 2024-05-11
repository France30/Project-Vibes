using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
	public State CurrentState { get; private set; }


	public void SetState(State state)
	{
		if (state == CurrentState) return;

		CurrentState?.PerformEndState(); //if current state exists, end it before beginning the next
		state.PerformBeginState(); //Initialize the next state
		CurrentState = state; //set the new state
	}

	protected virtual void Update()
	{
		if (CurrentState == null) return;

		CurrentState.CheckTransitionCondition();
	}

	protected virtual void FixedUpdate()
	{
		if (CurrentState == null) return;

		CurrentState.PerformState();
	}
}
