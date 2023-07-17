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

    private void Start()
    {
        if (_teleportPoints.Length > 0)
        {
            InitializeTurretBody();
        }

        _currentRotation = _startingRotation;
        _turret.localRotation = Quaternion.Euler(0, 0, _currentRotation);

        SetOnBossAttack(RotateTurret);
        SetOnBossAttackEnd(() => { if (!_isTeleporting) StartCoroutine(Teleport()); });

        SetAttack(() => { if (!_isAttackCoroutineRunning && !_isTeleporting) StartCoroutine(PlayAttack()); });
    }

    protected override void ActivateAbility()
    {
        switch(BossAttack.AbilityType)
        {
            case AbilityType.Projectile:
                BossAttack.FireProjectile(_turretSpawnPoint, _damage);
                break;
        }
    }

    private void InitializeTurretBody()
    {
        transform.position = _teleportPoints[_currentTeleportPoint].position;
        transform.rotation = _teleportPoints[_currentTeleportPoint].rotation;
        transform.localScale = _teleportPoints[_currentTeleportPoint].localScale;
    }

    private void RotateTurret()
    {
        _currentRotation += _degreesToRotate;

        if(_currentRotation >= _maxRotation)
        {
            _currentRotation = _startingRotation;
        }

        _turret.localRotation = Quaternion.Euler(0, 0, _currentRotation);
    }

    private IEnumerator Teleport()
    {
        _isTeleporting = true;
        //to play teleport animation
        AssignNewTeleportPoint();

        //yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        yield return new WaitForSeconds(0.5f); //for test only

        _currentRotation = _startingRotation;
        InitializeTurretBody();
        _turret.localRotation = Quaternion.Euler(0, 0, _currentRotation);
        //to play initialization animation
        _isTeleporting = false;
    }

    private void AssignNewTeleportPoint()
    {
        int newPoint = Random.Range(0, _teleportPoints.Length);
        
        if(newPoint == _currentTeleportPoint)
        {
            _currentTeleportPoint = (newPoint >= _teleportPoints.Length - 1) ? newPoint - 1 : newPoint + 1;
        }
        else
        {
            _currentTeleportPoint = newPoint;
        }
    }
}
