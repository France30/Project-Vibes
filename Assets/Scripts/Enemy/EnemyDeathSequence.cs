using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyBase))]
public class EnemyDeathSequence : MonoBehaviour
{
    [SerializeField] private string _deathAnimID = "Particle_Death";
    [Range(0,100)][SerializeField] private float _chanceForHealthDrop = 10f;
    [SerializeField] private string _healthDropID = "EnemyHealthDrop";

    private EnemyBase _enemyBase;
    private Animator _animator;

    public delegate IEnumerator AnimationEvent();
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
        OnAnimationEnd = null;
    }

    private void DeathSequence()
    {
        _enemyBase.enabled = false;
        StartCoroutine(PlayDeathSequence());
    }

    private IEnumerator PlayDeathSequence()
    {
        AttemptHealthDrop();

        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName(_deathAnimID));
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        if (OnAnimationEnd != null)
        {
            yield return StartCoroutine(OnAnimationEnd());
        }

        gameObject.SetActive(false);
    }

    private void AttemptHealthDrop()
    {
        int random = Random.Range(0, 100) + 1;
        if (random > _chanceForHealthDrop) return;

        GameObject healthDrop = ObjectPoolManager.Instance.GetPooledObject(_healthDropID);
        healthDrop.transform.position = transform.position;
        healthDrop.SetActive(true);
    }
}
