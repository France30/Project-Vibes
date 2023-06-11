using UnityEngine;

public class NormalGround : GroundEnemy
{
    protected override bool MoveCondition()
    {
        if (CurrentTarget == null) return false;

        return !IsTargetReached(CurrentTarget) && !IsAttacking;
    }

    protected override bool JumpCondition()
    {
        if (CurrentTarget == null) return false;

        //Disregard jump if on target
        if (IsTargetReached(CurrentTarget)) return false;

        //Jump if pathway is possible
        int wallColliders = Physics2D.OverlapBoxNonAlloc(_wallCheck.position, _localScale, 0, _overlapDetect, _whatIsPlatform);
        if (wallColliders > 0) return true;

        //Disregard jump if target is lower than or equal height to self
        bool isTargetBelow = CurrentTarget.position.y <= transform.position.y;
        if (isTargetBelow) return false;

        //Disregard jump if obstructed by a ceiling
        int ceilingColliders = Physics2D.BoxCastNonAlloc(transform.position, _localScale, 0, transform.up, _hitDetect, Mathf.Infinity, _whatIsPlatform);
        if (ceilingColliders > 0) return false;

        return false;
    }
}
