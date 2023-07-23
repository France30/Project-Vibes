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
            _animator.SetBool("Attack", true);
            RotateTurret();
        });

        SetOnBossAttackEnd(() => 
        {
            if (!_isTeleporting)
            {
                _animator.SetBool("Attack", false);
                _turret.localRotation = Quaternion.Euler(0, 0, 0);
                StartCoroutine(Teleport());
            }
        });
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
        AssignNewTeleportPoint();

        //yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        yield return null; //for test only

        InitializeTurretBase();
        //to play initialization animation
        _isTeleporting = false;
    }

    private void AssignNewTeleportPoint()
    {
        _currentTeleportPoint = (_currentTeleportPoint < _teleportPoints.Length - 1) ? _currentTeleportPoint + 1 : 0;
    }
}
