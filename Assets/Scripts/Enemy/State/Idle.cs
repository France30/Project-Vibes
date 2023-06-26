using UnityEngine;


public class Idle : State
{
    [SerializeField] private float _idleTime = 3f;

    private State _patrolState;
    private State _nextState;

    private float _currentTime = 0f;


    public override void PerformState()
    {
        base.PerformState();

        if (_patrolState == null) return;

        if (_currentTime < _idleTime)
            _currentTime += Time.fixedDeltaTime;
    }

    public override void CheckTransitionCondition()
    {
        if (_patrolState != null)
            CheckPatrolCondition();

        if (_nextState != null)
            CheckNextStateCondition();
    }

    private void CheckPatrolCondition()
    {
        if (_currentTime >= _idleTime) 
            _enemyBase.SetState(_patrolState);
    }

    private void CheckNextStateCondition()
    {
        if (_nextState.StateCondition)
        {
            _currentTime = 0;
            _enemyBase.SetState(_nextState);
        }
    }

    private void Start()
    {
        TryGetPatrolState();

        if (_patrolState == null)
            _idleTime = 0;

        TryGetNextState();
    }

    private void OnBecameInvisible()
    {
        _currentTime = 0;
    }

    private void TryGetPatrolState()
    {
        if (TryGetComponent<Patrol>(out Patrol patrol))
            _patrolState = patrol;

        Utilities.RemoveReferenceOfDisabledComponent<State>(ref _patrolState);
    }

    private void TryGetNextState()
    {
        if (TryGetComponent<Chase>(out Chase chase))
            _nextState = chase;
        else if (TryGetComponent<Attack>(out Attack attack))
            _nextState = attack;

        Utilities.RemoveReferenceOfDisabledComponent<State>(ref _nextState);
    }

    private void LateUpdate()
    {
        //Reset timer
        if (_enemyBase.CurrentState != this) 
            _currentTime = 0;
    }
}
