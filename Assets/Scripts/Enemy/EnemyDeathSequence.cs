using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBase))]
public class EnemyDeathSequence : MonoBehaviour
{
    private EnemyBase _enemyBase;
    private Animator _animator;

    public delegate void AnimationEvent();
    public event AnimationEvent OnAnimationStart;
    public event AnimationEvent OnAnimationEnd;

    private void Awake()
    {
        _enemyBase = GetComponent<EnemyBase>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _enemyBase.OnEnemyDeath += DeathSequence;
    }

    private void OnDisable()
    {
        _enemyBase.OnEnemyDeath -= DeathSequence;
    }

    private void DeathSequence()
    {
        _enemyBase.enabled = false;
        StartCoroutine(PlayDeathSequence());
    }

    private IEnumerator PlayDeathSequence()
    {
        OnAnimationStart?.Invoke();

        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        OnAnimationEnd?.Invoke();

        gameObject.SetActive(false);
    }
}
