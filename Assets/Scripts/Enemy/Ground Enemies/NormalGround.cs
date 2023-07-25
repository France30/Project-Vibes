using System;
using UnityEngine;

public class NormalGround : GroundEnemy
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(MoveCondition())
            _animator.SetFloat("Speed", Mathf.Abs(_moveSpeed));
        else
            _animator.SetFloat("Speed", 0);
    }

    protected override bool MoveCondition()
    {
        if (CurrentTarget == null) return false;

        return !IsTargetReached(CurrentTarget) && !IsAttacking && !IsIdle && !_spriteController.IsFlashing;
    }

    protected override bool JumpCondition()
    {
        if (CurrentTarget == null || _spriteController.IsFlashing) return false;

        //Disregard jump if on target
        if (IsTargetReached(CurrentTarget)) return false;

        //Jump if pathway is possible
        int wallColliders = Physics2D.OverlapBoxNonAlloc(_wallCheck.position, _spriteSize, 0, _overlapDetect, _whatIsPlatform);
        if (wallColliders > 0) return true;

        return false;
    }

    private void OnEnable()
    {
        _spriteController.OnFlashEvent += SetHurtAnimation;
    }

    private void OnDisable()
    {
        _spriteController.OnFlashEvent -= SetHurtAnimation;
    }

    private void SetHurtAnimation(bool isHurt)
    {
        AnimatorControllerParameter hurtParam = Array.Find<AnimatorControllerParameter>(_animator.parameters, animatorParam => animatorParam.name == "Hurt");
        if (hurtParam == null) return;

        _animator.SetBool(hurtParam.name, isHurt);
    }
}
