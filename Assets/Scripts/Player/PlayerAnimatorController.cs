using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator[] _animators;

    public void SetHurtParam(bool isHurt)
    {
        for(int i = 0; i < _animators.Length; i++)
        {
            _animators[i].SetBool("Hurt", isHurt);
        }
    }

    public void SetHealthParam(float health)
    {
        for (int i = 0; i < _animators.Length; i++)
        {
            _animators[i].SetFloat("Health", health);
        }
    }

    public void SetAttackParam(bool isAttack)
    {
        for (int i = 0; i < _animators.Length; i++)
        {
            _animators[i].SetBool("Attack", isAttack);
        }
    }

    public void SetJumpParam(bool isJump)
    {
        for (int i = 0; i < _animators.Length; i++)
        {
            _animators[i].SetBool("Jump", isJump);
        }
    }

    public void SetFallParam(bool isFall)
    {
        for (int i = 0; i < _animators.Length; i++)
        {
            _animators[i].SetBool("Fall", isFall);
        }
    }

    public void SetSpeedParam(float speed)
    {
        for (int i = 0; i < _animators.Length; i++)
        {
            _animators[i].SetFloat("Speed", speed);
        }
    }
}
