using System;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator[] _animators;

    private Action<bool> OnPlayerHurt;
    private Action<bool> OnPlayerAttack;
    private Action<bool> OnPlayerJump;
    private Action<bool> OnPlayerFall;

    private Action<float> OnPlayerHealthChange;
    private Action<float> OnPlayerMove;

    public void SetHurtParam(bool isHurt)
    {
        OnPlayerHurt?.Invoke(isHurt);
    }

    public void SetHealthParam(float health)
    {
        OnPlayerHealthChange?.Invoke(health);
    }

    public void SetAttackParam(bool isAttack)
    {
        OnPlayerAttack?.Invoke(isAttack);
    }

    public void SetJumpParam(bool isJump)
    {
        OnPlayerJump?.Invoke(isJump);
    }

    public void SetFallParam(bool isFall)
    {
        OnPlayerFall?.Invoke(isFall);
    }

    public void SetSpeedParam(float speed)
    {
        OnPlayerMove?.Invoke(speed);
    }

    private void Awake()
    {
        foreach (Animator animator in _animators)
            OnPlayerHurt += (bool isHurt) => animator.SetBool("Hurt", isHurt);

        foreach (Animator animator in _animators)
            OnPlayerAttack += (bool isAttack) => animator.SetBool("Attack", isAttack);

        foreach (Animator animator in _animators)
            OnPlayerJump += (bool isJump) => animator.SetBool("Jump", isJump);

        foreach (Animator animator in _animators)
            OnPlayerFall += (bool isFall) => animator.SetBool("Fall", isFall);

        foreach (Animator animator in _animators)
            OnPlayerHealthChange += (float health) => animator.SetFloat("Health", health);

        foreach (Animator animator in _animators)
            OnPlayerMove += (float speed) => animator.SetFloat("Speed", speed);
    }
}
