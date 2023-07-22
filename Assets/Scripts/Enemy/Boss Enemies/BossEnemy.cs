using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BossEnemy : EnemyBase
{
    [Header("Boss Attack Settings")]
    [SerializeField] private ScriptableObject _ability;
    [SerializeField] private ChordSet _chordSet;

    private int _currentChord = 0;
    private EnemyEvent OnBossAttackStart;
    private EnemyEvent OnBossAttackEnd;

    protected bool _isAttackCoroutineRunning = false;

    private EnemyEvent BossAttack;

    protected IBossAbility BossAbility { get { return _ability as IBossAbility; } }


    protected abstract void InitializeBossAttack();

    protected override void OnBecameInvisible()
    {
        //do nothing
    }

    protected void SetBossAttack(EnemyEvent bossAttack)
    {
        BossAttack = bossAttack;
    }

    protected void SetOnBossAttackStart(EnemyEvent onBossAttackStart)
    {
        OnBossAttackStart = onBossAttackStart;
    }

    protected void SetOnBossAttackEnd(EnemyEvent onBossAttackEnd)
    {
        OnBossAttackEnd = onBossAttackEnd;
    }

    protected IEnumerator PlayAttack()
    {
        _isAttackCoroutineRunning = true;

        ChordClip currentChordClip = _chordSet.ChordSetSO.chordClips[_currentChord];
        currentChordClip.source.Play();

        bool isChordPlaying = currentChordClip.clip != null;
        if (isChordPlaying)
        {
            OnBossAttackStart?.Invoke();
            BossAttack?.Invoke();
        }

        CheckIfSongDone();

        yield return new WaitForSeconds(_chordSet.ChordSetSO.time);

        if (_currentChord != 0) //continue chord progression
        {
            StartCoroutine(PlayAttack());
        }
        else
        {
            _isAttackCoroutineRunning = false;
            OnBossAttackEnd?.Invoke();
        }
    }

    protected virtual void Start()
    {
        InitializeBossAttack();
    }

    private void OnValidate()
    {
        if (!gameObject.activeInHierarchy) return;

        if (_ability is not IBossAbility)
        {
            throw new System.NullReferenceException("Ability Must Contain Type Of 'IBossAbility'");
        }
    }

    private void CheckIfSongDone()
    {
        bool isSongDone = _currentChord >= (_chordSet.ChordSetSO.chordClips.Length - 1);
        if (!isSongDone)
            _currentChord++;
        else
            _currentChord = 0;
    }
}
