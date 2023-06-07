using UnityEngine;
using UnityEngine.Events;

public class Attack : State
{
    [SerializeField] private float _playerDistanceToAttackState = 4f;

    [Space]
    public UnityEvent OnAttackEvent;

    private State _prevState;

    private bool _isAttacking = false;

    public override void PerformState()
    {
        bool wasAttacking = _isAttacking;
        _isAttacking = false;

        if (BeatSystemController.Instance.IsBeatPlaying)
        {
            _isAttacking = true;
            if (!wasAttacking)
            {
                OnAttackEvent?.Invoke();  //Events that should only happen once on the beat
                return;
            }
        }

        base.PerformState(); //Actions that need to happen during attack state
        _enemyBase.MoveToTargetDirection(GameController.Instance.Player.transform);
    }

    private void Start()
    {
        if (TryGetComponent<Chase>(out Chase chase))
            _prevState = chase;
        else if (TryGetComponent<Idle>(out Idle idle))
            _prevState = idle;
    }

    private void Update()
    {
        if (_prevState == null) return;

        CheckTransitionCondition();
    }

    private void CheckTransitionCondition()
    {
        //Enter Attack State
        bool attackStateCondition = _enemyBase.IsTargetReached(GameController.Instance.Player.transform, _playerDistanceToAttackState);
        if (attackStateCondition)
        {
            _enemyBase.SetState(this);
            return;
        }

        //Exit Attack State
        if (_enemyBase.CurrentState == this)
            _enemyBase.SetState(_prevState);
    }
}
