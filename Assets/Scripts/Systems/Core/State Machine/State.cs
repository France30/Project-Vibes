using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public abstract class State : MonoBehaviour
{
    protected EnemyBase _enemyBase;
    
    public delegate void Action();
    private Action StateAction;   //Callback delegate for state specific actions
    private Action BeginStateAction;
    private Action EndStateAction;

    public virtual bool StateCondition { get; }


    public virtual void PerformState()
    {
        StateAction?.Invoke();
    }

    public virtual void PerformBeginState()
    {
        BeginStateAction?.Invoke();
    }

    public virtual void PerformEndState()
    {
        EndStateAction?.Invoke();
    }

    public void SetAction(Action stateAction)
    {
        StateAction = stateAction;
    }

    public void SetBeginAction(Action beginAction)
    {
        BeginStateAction = beginAction;
    }

    public void SetEndAction(Action endAction)
    {
        EndStateAction = endAction;
    }

    public abstract void CheckTransitionCondition();

    private void Awake()
    {
        _enemyBase = GetComponent<EnemyBase>();
    }
}
