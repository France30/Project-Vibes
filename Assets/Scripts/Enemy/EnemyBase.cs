using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyBase : StateMachine, IDamageable
{
    [Header("Enemy Health")]
    [SerializeField] protected int _maxHealth = 1;
    [SerializeField] protected Image _healthBar = null;

    [Header("Enemy Movement")]
    [SerializeField] protected float _moveSpeed = 2f;

    protected Rigidbody2D _rb2D;
    protected Health _health;
    protected bool _isAttacking = false;

    private int _instanceID = 0;
    private bool _isFacingRight = true;

    public delegate void EnemyAttack();
    private EnemyAttack AttackEvent;

    public GameObject GameObject { get { return gameObject; } }
    public int InstanceID { get { return _instanceID; } }


    public void OnAttack()
    {
        AttackEvent?.Invoke();
    }

    public void TakeDamage(int value)
    {
        _health.CurrentHealth -= value;
        Debug.Log(InstanceID + " has been hit");

        if (_health.CurrentHealth <= 0) gameObject.SetActive(false);
    }

    public bool IsTargetReached(Transform target, float targetDistance = 1)
    {
        float distanceFromTarget = Vector2.Distance(transform.position, target.position);
        distanceFromTarget = Calculate.RoundedAbsoluteValue(distanceFromTarget);
        targetDistance = Calculate.RoundedAbsoluteValue(targetDistance);

        //Debug.Log(distanceFromTarget);
        return distanceFromTarget <= targetDistance;
    }

    public virtual void MoveToTargetDirection(Transform target)
    {
        bool isTargetRight = target.position.x > transform.position.x;
        if (isTargetRight && !_isFacingRight)
            Flip();
        if (!isTargetRight && _isFacingRight)
            Flip();
    }

    protected virtual void Flip()
    {
        _isFacingRight = !_isFacingRight;
        _moveSpeed *= -1;
    }

    protected void SetAttack(EnemyAttack enemyAttack)
    {
        AttackEvent = enemyAttack;
    }

    protected virtual void Awake()
    {
        _health = new Health(_maxHealth, _healthBar);
        _instanceID = gameObject.GetInstanceID();

        _rb2D = GetComponent<Rigidbody2D>();

        InitializeState();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        CheckForPlayerCollision();
        CheckForOtherEnemyCollision();
    }

    private void InitializeState()
    {
        if (TryGetComponent<Idle>(out Idle idle))
            SetState(idle);
        else if (TryGetComponent<Patrol>(out Patrol patrol))
            SetState(patrol);
        else if (TryGetComponent<Chase>(out Chase chase)) //for test
            SetState(chase);
        else if (TryGetComponent<Attack>(out Attack attack)) //for test
            SetState(attack);
    }

    private void CheckForPlayerCollision()
    {
        LayerMask player = LayerMask.GetMask("Player");
        if (Physics2D.OverlapBox(transform.position, transform.localScale, 0f, player))
            Debug.Log("Player Hit");
    }

    private void CheckForOtherEnemyCollision()
    {
        LayerMask enemy = LayerMask.GetMask("Enemy");
        var hitDetect = Physics2D.OverlapBox(transform.position, transform.localScale, 0f, enemy);
        if (hitDetect.gameObject != gameObject)
        {
            //enemies should move away from each other
            _rb2D.velocity = transform.position - hitDetect.transform.position;
        }
    }
}
