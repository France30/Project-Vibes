using UnityEngine;

public class FlyingEnemy : EnemyBase
{
	[Range(0, .3f)] [SerializeField] private float _movementSmoothing = 0.05f;

	[Header("Hover")]
	[SerializeField] private float _hoverSpeed = 1f;
	[SerializeField] private float _hoverDistance = 1f;

	[Header("Attack Configs")]
	[SerializeField] private ScriptableObject _ability;

	private Vector3 _velocity = Vector3.zero;
	private Vector3 _targetVelocity = Vector3.zero;
	private Vector3 _prevAttackVelocity = Vector3.zero;
	private Quaternion _targetRotation = Quaternion.identity;

	private float _currentHoverDistance;
	private bool _isHoveringUp = true;

	private IFlyingAbility FlyingAbility { get { return _ability as IFlyingAbility; } }


	public override void MoveToTargetDirection(Transform target)
	{
		if (IsAttacking && _animator.GetCurrentAnimatorStateInfo(0).IsName("Buzoo_DashBack")) return;

		base.MoveToTargetDirection(target);
		_targetRotation = EnemyUtilities.LookAtTarget(transform, target);

		//Disregard movement if attacking
		if (IsAttacking) return;

		_targetVelocity = EnemyUtilities.FreeMoveTowardsTarget(ref _moveSpeed, transform, target);
	}

	private void OnValidate()
	{
		if (!gameObject.activeInHierarchy) return;

		if(_ability is not IFlyingAbility)
		{
			throw new System.NullReferenceException("Ability Must Contain Type Of 'IFlyingAbility'");
		}
	}

	protected override void Start()
	{
		base.Start();

		if (TryGetComponent<Idle>(out Idle idle))
			idle.SetAction(Hover);

		if (TryGetComponent<Attack>(out Attack attack))
		{
			attack.SetBeginAction(() =>
			{
				//prevent dash back when transitioning into attack state
				_prevAttackVelocity = Vector3.zero;
			});

			attack.SetEndAction(() =>
			{
				//reset attack animations when ending attack state
				_animator.SetBool("Attack", false);
				_animator.SetBool("ReadyAttack", false);
			});

			attack.SetAction(() =>
			{
				_animator.SetBool("Attack", false);
				_animator.SetBool("ReadyAttack", true);
				DashBackAfterAttack();
			});
		}

		SetAttack(() => 
		{
			_targetVelocity = FlyingAbility.ApplyAttackVelocity(_moveSpeed, transform);
			_prevAttackVelocity = _targetVelocity;
			_animator.SetBool("Attack", true);
			_animator.SetBool("ReadyAttack", false);
		});
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		_rb2D.velocity = Vector3.SmoothDamp(_rb2D.velocity, _targetVelocity, ref _velocity, _movementSmoothing);
		transform.rotation = _targetRotation;

		if (IsIdle) LookAhead();
		_targetVelocity = Vector3.zero;
	}

	protected override void Flip()
	{
		if (IsAttacking && _animator.GetCurrentAnimatorStateInfo(0).IsName("Buzoo_DashBack")) return;

		base.Flip();
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		theScale.y *= -1;
		transform.localScale = theScale;

		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.flipX = !spriteRenderer.flipX;
	}

	private void Hover()
	{
		float hover = _hoverSpeed * Time.fixedDeltaTime;
		_currentHoverDistance += hover;
		if (_currentHoverDistance >= _hoverDistance && _isHoveringUp)
			FlipHover();
		else if(_currentHoverDistance <= _hoverDistance && !_isHoveringUp)
			FlipHover();

		_targetVelocity = new Vector2(0, hover);
	}

	private void FlipHover()
	{
		_isHoveringUp = !_isHoveringUp;
		_hoverDistance *= -1;
		_hoverSpeed *= -1;
	}

	private void LookAhead()
	{
		if (transform.localScale.x > 0)
			_targetRotation = Quaternion.identity;
		if (transform.localScale.x < 0)
			_targetRotation = Quaternion.Euler(0, 0, 180);
	}

	private void DashBackAfterAttack()
	{
		if (_prevAttackVelocity != Vector3.zero)
		{
			_rb2D.velocity = Vector2.zero;
			_targetVelocity = (_prevAttackVelocity / 2) * -1;
			_prevAttackVelocity = Vector3.zero; //reset the value
		}
	}
}
