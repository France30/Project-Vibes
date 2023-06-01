using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGround : GroundEnemy
{
    protected override bool MoveCondition()
    {
        if (CurrentTarget == null) return false;

        return !IsTargetReached(CurrentTarget);
    }

    protected override bool JumpCondition()
    {
        if (CurrentTarget == null) return false;

        //Disregard jump if on target
        if (IsTargetReached(CurrentTarget)) return false;

        //Jump if pathway is possible
        bool isThereWall = Physics2D.OverlapBox(_wallCheck.position, _localScale, 0, _whatIsPlatform);
        if (isThereWall) return true;

        //Disregard jump if target is lower than or equal height to self
        bool isTargetBelow = CurrentTarget.position.y <= transform.position.y;
        if (isTargetBelow) return false;

        //Disregard jump if obstructed by a ceiling
        bool isBelowPlatform = Physics2D.BoxCast(_ceilingCheck.position, _ceilingBoxCastSize, 0, transform.up, Mathf.Infinity, _whatIsPlatform);
        if (isBelowPlatform) return false;

        Vector2 _platformCheck = new Vector2(_wallCheck.position.x, _wallCheck.position.y + _localScale.y + 0.5f);
        bool isTherePlatform = Physics2D.OverlapBox(_platformCheck, _localScale, 0, _whatIsPlatform);
        if (isTherePlatform) return true;

        return false;
    }
}
