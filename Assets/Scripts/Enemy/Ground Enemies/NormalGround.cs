using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGround : GroundEnemy
{
    protected override bool MoveCondition()
    {
        if (CurrentTarget == null) return false;

        //Only move if not on target and not attacking
        return !IsTargetReached(CurrentTarget) && !_isAttacking;
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
        bool isBelowPlatform = Physics2D.BoxCast(transform.position, _localScale, 0, transform.up, Mathf.Infinity, _whatIsPlatform);
        if (isBelowPlatform) return false;

        return false;
    }
}
