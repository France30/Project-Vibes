using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Patrol : State
{
    [SerializeField] private float _playerDistanceToNextState = 10f;
    [SerializeField] private Transform[] _wayPoints;

    private int _currentWayPoint = 0;

    private State _nextState;

    public override void PerformState()
    {
        base.PerformState();

        Transform currentPatrol = _wayPoints[_currentWayPoint];
        if (_enemyBase.IsTargetReached(currentPatrol))
            currentPatrol = GoToNextWayPoint();

        _enemyBase.MoveToTargetDirection(currentPatrol);
    }

    private Transform GoToNextWayPoint()
    {
        _currentWayPoint++;

        if (_currentWayPoint > _wayPoints.Length - 1)
            _currentWayPoint = 0;

        return _wayPoints[_currentWayPoint];
    }

    private void Start()
    {
        if (TryGetComponent<Chase>(out Chase chase))
            _nextState = chase;
    }

    private void Update()
    {
        if (_nextState == null) return;

        CheckTransitionCondition();
    }

    private void CheckTransitionCondition()
    {
        //Exit Patrol State
        bool nextStateCondition = _enemyBase.IsTargetReached(GameController.Instance.Player.transform, _playerDistanceToNextState);
        if (nextStateCondition)
        {
            _enemyBase.SetState(_nextState);
            return;
        }

        //Enter Patrol State
        if (_enemyBase.CurrentState == _nextState)
            _enemyBase.SetState(this);
    }
}
