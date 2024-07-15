using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossTurret : BossEnemy
{
	[SerializeField] private float _startingRotation = -90f;
	[SerializeField] private float _maxRotation = 90f;
	[SerializeField] private float _degreesToRotate = 25f;

	[Space]
	[SerializeField] private Transform[] _teleportPoints;

	[SerializeField] private Transform _turret;
	[SerializeField] private Transform _turretSpawnPoint;

	[Space]
	[SerializeField] private Collider2D[] _IdleHitbox;
	[SerializeField] private Collider2D[] _AttackHitbox;

	[Header("Phase 2 Mob Spawn")]
	[SerializeField] private string _enemySpawnID;
	[SerializeField] private int _randomRangeToSpawn;

	private float _currentRotation = 0f;
	private int _currentTeleportPoint = 0;

	private bool _isTeleporting = false;


	public override void TakeDamage(int value)
	{
		base.TakeDamage(value);

		if (_turret.TryGetComponent<SpriteController>(out SpriteController spriteController))
		{
			if (!spriteController.IsFlashing) 
			{
				StartCoroutine(spriteController.Flash());
			}
		}
	}

	protected override void InitializeBossAttack()
	{
		switch (BossAbility.AbilityType)
		{
			case AbilityType.Projectile:
				SetBossAttack(() => BossAbility.FireProjectile(_turretSpawnPoint));
				break;
		}
	}

	protected override void Start()
	{
		base.Start();

		InitializeHitBox();
		InitializeTurretBase();
		_turret.localRotation = Quaternion.Euler(0, 0, 0);

		SetAttack(() => 
		{
			if (!_isAttackCoroutineRunning && !_isTeleporting && !_isCooldown)
			{
				_currentRotation = _startingRotation;
				_turret.localRotation = Quaternion.Euler(0, 0, _currentRotation);

				StartCoroutine(PlayAttack());
			}
		});

		SetOnBossAttackStart(() =>
		{
			//Initialize Turret Base
			transform.localRotation = Quaternion.Euler(0, 0, 0);
			transform.localScale = Vector3.one;

			_animator.SetBool("Attack", true);
			Utilities.EnableComponents<Collider2D>(_IdleHitbox, false);
			Utilities.EnableComponents<Collider2D>(_AttackHitbox, true);
			RotateTurret();
		});

		SetOnBossAttackEnd(() => 
		{
			if (!_isTeleporting)
			{
				_animator.SetBool("Attack", false);
				_turret.localRotation = Quaternion.Euler(0, 0, 0);
				_currentRotation = _startingRotation;

				Utilities.EnableComponents<Collider2D>(_IdleHitbox, true);
				Utilities.EnableComponents<Collider2D>(_AttackHitbox, false);

				StartCoroutine(Teleport());

				if(_isCooldown)
				{
					var currentTeleportPoint = _teleportPoints[_currentTeleportPoint];
					transform.localRotation = Quaternion.Euler(0, 0, currentTeleportPoint.localEulerAngles.z);
					if(transform.localRotation.z > 0)
						transform.localScale = new Vector3(1, currentTeleportPoint.localScale.x * -1f, 1);
				}
			}
		});
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent<Player>(out Player player))
		{
			if (player.IsInvulnerable) return;

			player.TakeDamage(_damage, EnemyUtilities.GetCollisionDirection(transform, collision));
		}
	}

	private void InitializeTurretBase()
	{
		transform.SetParent(_teleportPoints[_currentTeleportPoint], false);
		transform.position = _teleportPoints[_currentTeleportPoint].position;
		_turretSpawnPoint.localScale = transform.parent.localScale;
	}

	private void RotateTurret()
	{
		_turret.localRotation = Quaternion.Euler(0, 0, _currentRotation);

		_currentRotation += _degreesToRotate;
		if (_currentRotation > _maxRotation)
		{
			_currentRotation = _startingRotation;
		}
	}

	private IEnumerator Teleport()
	{
		_isTeleporting = true;
		//to play teleport animation
		_currentTeleportPoint = (_currentTeleportPoint < _teleportPoints.Length - 1) ? _currentTeleportPoint + 1 : 0;
		//yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
		yield return null; //for test only

		InitializeTurretBase();

		if (_health.CurrentHealth <= _health.MaxHealth / 2)
			SpawnMob();
		//to play initialization animation
		_isTeleporting = false;
	}

	private void InitializeHitBox()
	{
		Utilities.EnableIsTriggerOnColliders(_IdleHitbox);
		Utilities.EnableIsTriggerOnColliders(_AttackHitbox);

		Utilities.EnableComponents<Collider2D>(_IdleHitbox, true);
		Utilities.EnableComponents<Collider2D>(_AttackHitbox, false);
	}

	private void SpawnMob()
    {
		int amount = Random.Range(1, _randomRangeToSpawn + 1);
		for(int i = 0; i < amount; i++)
        {
			GameObject mob = ObjectPoolManager.Instance.GetPooledObject(_enemySpawnID);
			if (mob == null) break;

			EnemyBase enemy = mob.GetComponent<EnemyBase>();
			if(enemy.GetComponent<PooledObjectItem>() == null)
            {
				enemy.GetComponent<EnemyDeathSequence>().OnAnimationEnd += () => 
				{
					ObjectPoolManager.Instance.DespawnGameObject(mob); 
				};
            }

			mob.transform.position = _teleportPoints[_currentTeleportPoint].position;
			mob.SetActive(true);
		}
    }
}
