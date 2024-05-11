using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BossEnemy : EnemyBase
{
	[Header("Boss Attack Settings")]
	[SerializeField] private ScriptableObject _ability;
	[SerializeField] private float _cooldown = 1f;

	private ChordSet[] _chordSets;

	private int _currentChord = 0;
	private int _currentChordSet = 0;
	private EnemyDeathSequence _enemyDeathSequence;
	private EnemyEvent OnBossAttackStart;
	private EnemyEvent OnBossAttackEnd;

	protected bool _isAttackCoroutineRunning = false;
	protected bool _isCooldown = false;

	private EnemyEvent BossAttack;

	public EnemyDeathSequence EnemyDeathSequence { 
		get 
		{
			if (_enemyDeathSequence == null)
				_enemyDeathSequence = GetComponent<EnemyDeathSequence>();

			return _enemyDeathSequence;
		} 
	}

	protected IBossAbility BossAbility { get { return _ability as IBossAbility; } }


	protected override void OnBecameInvisible()
	{
		//do nothing
	}

	protected abstract void InitializeBossAttack();

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

		ChordSetSO currentChordSetSO = _chordSets[_currentChordSet].ChordSetSO;
		ChordClip currentChordClip = currentChordSetSO.chordClips[_currentChord];
		currentChordClip.source.Play();

		bool isChordPlaying = currentChordClip.clip != null;
		if (isChordPlaying)
		{
			OnBossAttackStart?.Invoke();
			BossAttack?.Invoke();
		}

		CheckIfSongDone();

		yield return new WaitForSeconds(currentChordSetSO.time);

		if (_currentChord != 0) //continue chord progression
		{
			StartCoroutine(PlayAttack());
		}
		else if (_currentChordSet < _chordSets.Length - 1)
		{
			_currentChordSet++;
			OnBossAttackEnd?.Invoke();
			StartCoroutine(PlayAttack());
		}
		else
		{
			_currentChordSet = 0;
			_isAttackCoroutineRunning = false;
			StartCoroutine(Cooldown());
		}
	}

	protected override void CheckForPlayerCollision()
	{
		//we assume bosses will need manual/fixed hitboxes
	}

	protected override void Awake()
	{
		base.Awake();
		_enemyDeathSequence = GetComponent<EnemyDeathSequence>();
		_chordSets = GetComponentsInChildren<ChordSet>();
	}

	protected virtual void Start()
	{       
		InitializeBossAttack();

		Physics2D.IgnoreLayerCollision(gameObject.layer, GameController.Instance.Player.gameObject.layer, false);
	}

	protected virtual void OnEnable()
	{
		GameController.Instance.Player.OnPlayerHurt += IgnorePlayerBossCollision;
	}

	protected virtual void OnDisable()
	{
		if (GameController.Instance == null) return;

		GameController.Instance.Player.OnPlayerHurt -= IgnorePlayerBossCollision;
	}

	private void IgnorePlayerBossCollision(bool isDisable)
	{
		Physics2D.IgnoreLayerCollision(gameObject.layer, GameController.Instance.Player.gameObject.layer, isDisable);
	}

	private IEnumerator Cooldown()
	{
		_isCooldown = true;
		OnBossAttackEnd?.Invoke();

		yield return new WaitForSeconds(_cooldown);

		_isCooldown = false;
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
		bool isSongDone = _currentChord >= (_chordSets[_currentChordSet].ChordSetSO.chordClips.Length - 1);
		if (!isSongDone)
			_currentChord++;
		else
			_currentChord = 0;
	}
}
