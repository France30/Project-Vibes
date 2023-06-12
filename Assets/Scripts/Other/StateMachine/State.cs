using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public abstract class State : MonoBehaviour
{
    protected EnemyBase _enemyBase;
    
    public delegate void Action();
    private Action StateAction;   //Callback delegate for state specific actions


    public virtual void PerformState()
    {
        StateAction?.Invoke();
    }

    public void SetAction(Action stateAction)
    {
        StateAction = stateAction;
    }

    private void Awake()
    {
        _enemyBase = GetComponent<EnemyBase>();
    }
}
