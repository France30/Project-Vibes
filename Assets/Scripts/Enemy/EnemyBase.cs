using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(EnemyDeathSequence))]
public abstract class EnemyBase : StateMachine, IDamageable
{
	[Header("Enemy Configs")]
	[SerializeField] private EnemyPermaDeathSO _enemyPermaDeath;
	[SerializeField] private GateEvent _gateEvent;

	[Header("Enemy Health")]
	[SerializeField] protected int _maxHealth = 1;
	[SerializeField] protected Image _healthBar = null;
	[SerializeField] protected Sprite[] _healthBarSprite;

	[Header("Enemy Damage")]
	[SerializeField] protected int _damage = 1;

	[Header("Enemy Movement")]
	[SerializeField] protected float _moveSpeed = 2f;

	protected Vector2 _spriteSize;
	protected Rigidbody2D _rb2D;
	protected Health _health;
	protected Animator _animator;
	protected SpriteController _spriteController;

	private int _instanceID = 0;
	private bool _isFacingRight = true;
	private Collider2D[] _playerCollider = new Collider2D[1];

	public delegate void EnemyEvent();
	public event EnemyEvent OnEnemyDeath;
	private EnemyEvent EnemyAttack;

	public int InstanceID { get { return _instanceID; } }
	public int MaxHealth { get { return (int)_health.MaxHealth; } }
	public EnemyPermaDeathSO PermaDeath {  get { return _enemyPermaDeath; } }
	protected bool IsAttacking { get; private set; }
	protected bool IsIdle { get; private set; }


	public void OnAttack()
	{
		EnemyAttack?.Invoke();
	}

	public virtual void TakeDamage(int value)
	{
		if (_health.CurrentHealth <= 0) return;

		_health.CurrentHealth -= value;
		_animator.SetFloat("Health", _health.CurrentHealth);

		if(_healthBar != null)
        {
			_healthBar.sprite = _healthBarSprite[(int)_health.CurrentHealth];
		}

		if (_health.CurrentHealth <= 0)
		{
			OnEnemyDeath?.Invoke();
			return;
		}

		if (!_spriteController.IsFlashing)
			StartCoroutine(_spriteController.Flash());
	}

	public bool IsTargetReached(Transform target, float targetDistance = 1)
	{
		float distanceFromTarget = Vector2.Distance(transform.position, target.position);
		distanceFromTarget = Calculate.RoundedAbsoluteValue(distanceFromTarget);
		targetDistance = Calculate.RoundedAbsoluteValue(targetDistance);

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

	protected void SetAttack(EnemyEvent enemyAttack)
	{
		EnemyAttack = enemyAttack;
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

	protected virtual void OnBecameVisible()
	{
		this.enabled = true;
	}

	protected virtual void OnBecameInvisible()
	{
		this.enabled = false;
	}

	protected virtual void Awake()
    {
		_spriteController = GetComponent<SpriteController>();
		_rb2D = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
	}

	protected virtual void Start()
	{
		_spriteSize = _spriteController.SpriteSize;
		_enemyPermaDeath?.InitializeEnemyPermaDeath(this, _gateEvent);

		_health = new Health(_maxHealth);
		_instanceID = gameObject.GetInstanceID();

		_animator.SetFloat("Health", _health.CurrentHealth);

		InitializeState();
		this.enabled = false;
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

	//Player should be able to walk through enemies when damaged
	//To achieve this, Collision with colliders is turned off in the collision matrix. Instead, we detect collision via casting
	//Note: More complicated enemy designs may create awkward collision, since this logic scales the hitbox with the sprite.
	//Note: Bosses will either require separate collision logic or separate animations depending on the design
	protected virtual void CheckForPlayerCollision()
	{
		LayerMask playerLayer = LayerMask.GetMask("Player");
		int hitDetect = Physics2D.OverlapBoxNonAlloc(transform.position, _spriteSize, 0, _playerCollider, playerLayer);
		if (hitDetect > 0)
		{
			Player player = GameController.Instance.Player;
			if (player.IsInvulnerable) return;

			player.TakeDamage(_damage, EnemyUtilities.GetCollisionDirection(transform, _playerCollider[0]));
		}
	}

	protected virtual void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, _spriteSize);
	}
}
