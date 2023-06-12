using UnityEngine;
using UnityEngine.Events;

public class Attack : State
{
    [SerializeField] private float _playerDistanceToAttackState = 4f;

    [Space]
    public UnityEvent OnAttackEvent;

    private Transform _player;

    private State _prevState;

    private bool _isAttacking = false;

    public override bool StateCondition { get { return _enemyBase.IsTargetReached(_player, _playerDistanceToAttackState); } }


    public override void PerformState()
    {
        bool wasAttacking = _isAttacking;
        _isAttacking = false;

        if (!wasAttacking)
        {
            base.PerformState(); //Actions that need to happen during attack state
            _enemyBase.MoveToTargetDirection(_player);
        }

        if (BeatSystemController.Instance.IsBeatPlaying)
        {
            _isAttacking = true;
            if (!wasAttacking)
            {
                OnAttackEvent?.Invoke();  //Events that should only happen once on the beat
            }
        }
    }

    public override void CheckTransitionCondition()
    {
        if (_prevState == null) return;

        if (!StateCondition)
            _enemyBase.SetState(_prevState);
    }

    private void Start()
    {
        _player = GameController.Instance.Player.transform;

        TryGetPrevState();
    }

    private void TryGetPrevState()
    {
        if (TryGetComponent<Chase>(out Chase chase))
            _prevState = chase;
        else if (TryGetComponent<Idle>(out Idle idle))
            _prevState = idle;
        else if (TryGetComponent<Patrol>(out Patrol patrol))
            _prevState = patrol;
    }
}
