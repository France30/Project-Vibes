using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyBase : StateMachine, IDamageable
{
    [Header("Enemy Health")]
    [SerializeField] protected int _maxHealth = 1;
    [SerializeField] protected Image _healthBar = null;

    [Header("Enemy Damage")]
    [SerializeField] protected int _damage = 1;

    [Header("Enemy Movement")]
    [SerializeField] protected float _moveSpeed = 2f;

    protected Vector2 _spriteSize;
    protected Rigidbody2D _rb2D;
    protected Health _health;

    private int _instanceID = 0;
    private bool _isFacingRight = true;
    private Collider2D[] _playerCollider = new Collider2D[1];
    private SpriteController _spriteController;

    public delegate void EnemyAttack();
    private EnemyAttack AttackEvent;

    public GameObject GameObject { get { return gameObject; } }
    public int InstanceID { get { return _instanceID; } }
    protected bool IsAttacking { get; private set; }
    protected bool IsIdle { get; private set; }


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

    private void Awake()
    {
        _health = new Health(_maxHealth, _healthBar);
        _instanceID = gameObject.GetInstanceID();

        _spriteSize = GetComponent<SpriteRenderer>().sprite.bounds.size;
        _spriteController = GetComponent<SpriteController>();
        _rb2D = GetComponent<Rigidbody2D>();

        InitializeState();
    }

    protected override void Update()
    {
        base.Update();

        IsIdle = CurrentState is Idle;
        IsAttacking = CurrentState is Attack;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        CheckForPlayerCollision();
    }

    private void InitializeState()
    {
        if (TryGetComponent<Idle>(out Idle idle))
            SetState(idle);
        else if (TryGetComponent<Patrol>(out Patrol patrol))
            SetState(patrol);
        else if (TryGetComponent<Chase>(out Chase chase))
            SetState(chase);
        else if (TryGetComponent<Attack>(out Attack attack))
            SetState(attack);
    }

    private void CheckForPlayerCollision()
    {
        LayerMask playerLayer = LayerMask.GetMask("Player");
        int hitDetect = Physics2D.OverlapBoxNonAlloc(transform.position, _spriteSize, 0, _playerCollider, playerLayer);
        if (hitDetect > 0)
        {
            Player player = GameController.Instance.Player;
            player.TakeDamage(_damage, EnemyUtilities.GetCollisionDirection(transform, _playerCollider[0]));
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, _spriteSize);
    }
}
