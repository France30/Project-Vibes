using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Normal Ground Enemy", menuName = "Enemy/Ground Enemy/Normal Ground")]
public class NormalGroundType : GroundEnemyType
{
    public override void RegisterEvents()
    {
        //do nothing
    }

    public override void UnregisterEvents()
    {
        //do nothing
    }

    public override bool MoveCondition()
    {
        if (CurrentTarget == null) return false;

        return !_groundEnemy.IsTargetReached(CurrentTarget);
    }

    public override bool JumpCondition()
    {
        if (CurrentTarget == null) return false;

        //No need to jump if target has been reached
        if (_groundEnemy.IsTargetReached(CurrentTarget)) return false;

        //Always jump if there is a wall obstructing the path
        bool isThereWall = Physics2D.OverlapBox(_wallCheck.position, _wallBoxCastSize, 0, _whatIsPlatform);
        if (isThereWall) return true;

        //No need to jump if target is not on a higher position than the enemy
        bool isTargetAbove = CurrentTarget.position.y > _transform.position.y;
        if (!isTargetAbove) return false;

        //Do not jump if enemy is right under a platform
        bool isBelowPlatform = Physics2D.BoxCast(_ceilingCheck.position, _ceilingBoxCastSize, 0, _transform.up, Mathf.Infinity, _whatIsPlatform);
        if (isBelowPlatform) return false;

        Vector2 _platformCheck = new Vector2(_wallCheck.position.x, _wallCheck.position.y + _wallBoxCastSize.y + 0.5f);
        bool isTherePlatform = Physics2D.OverlapBox(_platformCheck, _wallBoxCastSize, 0, _whatIsPlatform);
        if (isTherePlatform) return true;

        return false;
    }
}
