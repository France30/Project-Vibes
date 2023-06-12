using UnityEngine;

public class Chase : State
{
    [Range(0,Mathf.Infinity)][SerializeField] private float _playerDistanceToChaseState = 10f;

    private Transform _player;

    private State _attackState;
    private State _prevState;

    public override bool StateCondition { get { return _enemyBase.IsTargetReached(_player, _playerDistanceToChaseState); } }


    public override void PerformState()
    {
        base.PerformState();
        _enemyBase.MoveToTargetDirection(_player);
    }

    public override void CheckTransitionCondition()
    {
        if (_attackState != null)
            CheckAttackCondition();

        if (_prevState != null)
            CheckPrevStateCondition();
    }

    private void CheckAttackCondition()
    {
        if (_attackState.StateCondition)
            _enemyBase.SetState(_attackState);
    }

    private void CheckPrevStateCondition()
    {
        if (!StateCondition)
            _enemyBase.SetState(_prevState);
    }

    private void Start()
    {
        _player = GameController.Instance.Player.transform;

        TryGetAttackState();
        TryGetPrevState();
    }

    private void TryGetAttackState()
    {
        if (TryGetComponent<Attack>(out Attack attack))
            _attackState = attack;
    }

    private void TryGetPrevState()
    {
        if (TryGetComponent<Idle>(out Idle idle))
            _prevState = idle;
        else if (TryGetComponent<Patrol>(out Patrol patrol))
            _prevState = patrol;
        else
            _playerDistanceToChaseState = Mathf.Infinity;
    }
}
