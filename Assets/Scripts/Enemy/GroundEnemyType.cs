using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class GroundEnemyType : ScriptableObject
{
    //public int maxHealth = 1;
    public float moveSpeed = 15f;

    protected Transform _transform;
    protected Transform _wallCheck;
    protected Vector2 _wallBoxCastSize;
    protected Transform _ceilingCheck;
    protected Vector2 _ceilingBoxCastSize;
    protected LayerMask _whatIsPlatform;

    protected GroundEnemy _groundEnemy;
    protected Rigidbody2D _rigidbody2D;

    public Transform CurrentTarget { get; set; }
    public float FallingThreshold { get; set; }

    public abstract void RegisterEvents();
    public abstract void UnregisterEvents();
    public abstract bool JumpCondition();
    public abstract bool MoveCondition();

    public void InitializeEnemy(GroundEnemy groundEnemy)
    {
        if (_groundEnemy != null) return;

        _groundEnemy = groundEnemy;
        _transform = groundEnemy.transform;
        _rigidbody2D = groundEnemy.GetComponent<Rigidbody2D>();
    }

    public void InitializeChecks(Transform wallCheck, Transform ceilingCheck, Vector2 wallBoxCastSize, Vector2 ceilingBoxCastSize, LayerMask whatIsPlatform)
    {
        //Initialize wall check
        _wallCheck = wallCheck;
        _wallBoxCastSize = wallBoxCastSize;

        //Initialize ceiling check
        _ceilingCheck = ceilingCheck;
        _ceilingBoxCastSize = ceilingBoxCastSize;

        _whatIsPlatform = whatIsPlatform;
    }

}
