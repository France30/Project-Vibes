using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BossEnemy : EnemyBase
{
    [Header("Boss Attack Settings")]
    [SerializeField] private ScriptableObject _ability;
    [SerializeField] private ChordSet _chordSet;

    private int _currentChord = 0;
    private EnemyEvent OnBossAttack;
    private EnemyEvent OnBossAttackEnd;

    protected bool _isAttackCoroutineRunning = false;

    protected IBossAttack BossAttack { get { return _ability as IBossAttack; } }


    protected abstract void ActivateAbility();
    protected override void OnBecameInvisible()
    {
        //do nothing
    }

    protected void SetOnBossAttack(EnemyEvent onBossAttack)
    {
        OnBossAttack = onBossAttack;
    }

    protected void SetOnBossAttackEnd(EnemyEvent onBossAttackEnd)
    {
        OnBossAttackEnd = onBossAttackEnd;
    }

    protected IEnumerator PlayAttack()
    {
        _isAttackCoroutineRunning = true;
        //Debug.Log("Boss is Attacking");

        ChordClip currentChordClip = _chordSet.ChordSetSO.chordClips[_currentChord];
        currentChordClip.source.Play();

        bool isChordPlaying = currentChordClip.clip != null;
        if (isChordPlaying)
        {
            OnBossAttack?.Invoke();
            ActivateAbility();
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

    private void OnValidate()
    {
        if (!gameObject.activeInHierarchy) return;

        if (_ability is not IBossAttack)
        {
            throw new System.NullReferenceException("Ability Must Contain Type Of 'IBossAttack'");
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
