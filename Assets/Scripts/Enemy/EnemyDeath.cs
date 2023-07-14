using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBase))]
public class EnemyDeath : MonoBehaviour
{
    private EnemyBase _enemyBase;
    private Animator _animator;


    private void Awake()
    {
        _enemyBase = GetComponent<EnemyBase>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _enemyBase.OnEnemyDeath += OnDeath;
    }

    private void OnDisable()
    {
        _enemyBase.OnEnemyDeath -= OnDeath;
    }

    private void OnDeath()
    {
        _enemyBase.enabled = false;
        StartCoroutine(DisableGameObjectOnAnimationEnd());
    }

    private IEnumerator DisableGameObjectOnAnimationEnd()
    {
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        gameObject.SetActive(false);
    }
}
