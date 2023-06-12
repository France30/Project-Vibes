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
        int wallColliders = Physics2D.OverlapBoxNonAlloc(_wallCheck.position, _localScale, 0, _overlapDetect, _whatIsPlatform);
        if (wallColliders > 0) return true;

        return false;
    }
}
