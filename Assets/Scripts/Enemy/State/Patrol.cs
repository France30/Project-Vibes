using UnityEngine;

public class Patrol : State
{
    [SerializeField] private Transform[] _wayPoints;
    [Range(1,10)][SerializeField] private float _targetDistanceFromWayPoint = 1f;

    private State _idleState;
    private State _nextState;

    private int _currentWayPoint = 0;
    private bool _isWayPointReached = false;


    public override void PerformState()
    {
        Transform currentPatrol = _wayPoints[_currentWayPoint];

        if (!_isWayPointReached)
        {
            base.PerformState();
            _enemyBase.MoveToTargetDirection(currentPatrol);
        }

        if (_enemyBase.IsTargetReached(currentPatrol,_targetDistanceFromWayPoint))
        {
            UpdateWayPoint();

            if (_idleState == null) return;
                
            _isWayPointReached = true;
        }
    }

    public override void CheckTransitionCondition()
    {
        if (_idleState != null)
            CheckIdleCondition();

        if (_nextState != null)
            CheckNextStateCondition();
    }

    private void CheckIdleCondition()
    {
        if (_isWayPointReached)
            _enemyBase.SetState(_idleState);
    }

    private void CheckNextStateCondition()
    {
        if (_nextState.StateCondition)
            _enemyBase.SetState(_nextState);
    }

    private void UpdateWayPoint()
    {
        _currentWayPoint++;

        if (_currentWayPoint > _wayPoints.Length - 1)
            _currentWayPoint = 0;
    }

    private void Start()
    {
        TryGetIdleState();
        TryGetNextState();
    }

    private void TryGetIdleState()
    {
        if (TryGetComponent<Idle>(out Idle idle))
            _idleState = idle;
    }

    private void TryGetNextState()
    {
        if (TryGetComponent<Chase>(out Chase chase))
            _nextState = chase;
        else if (TryGetComponent<Attack>(out Attack attack))
            _nextState = attack;
    }

    private void LateUpdate()
    {
        //Reset flag
        if (_enemyBase.CurrentState != this)
            _isWayPointReached = false;

        if (_enemyBase.CurrentState == _nextState)
            _currentWayPoint = 0;
    }
}
