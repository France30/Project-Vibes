using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	[SerializeField] private GameObject _attackObject;
	[SerializeField] private PlayerChords _playerChords;
	[SerializeField] private float _penaltyCooldown = 3.0f;

	private AttackObjectController _attackObjectController;
	private Animator _animator;

	private bool _isAttackCoroutineRunning = false;
	private bool _didPlayerMissBeat = false;
	private int _currentChord = 0;

	private WaitForSeconds _waitForPenaltyCooldown;

	public PlayerChords PlayerChords { get { return _playerChords; } }

	private void Awake()
	{
		_attackObjectController = _attackObject.GetComponent<AttackObjectController>();
		_animator = GetComponent<Animator>();

		if (_attackObject.activeSelf)
			_attackObject.SetActive(false);
	}

	private void OnDisable()
	{
		_currentChord = 0;
	}

	// Update is called once per frame
	private void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if (_didPlayerMissBeat) return;

			if(!BeatSystemController.Instance.IsBeatPlaying)
			{
				StartCoroutine(AttackNotOnBeat());
				return;
			}

			if(!_isAttackCoroutineRunning)
				StartCoroutine(PlayAttack());
		}

		if (Input.GetButtonUp("Fire1"))
			_currentChord = 0;
	}

	private IEnumerator AttackNotOnBeat()
	{
		_didPlayerMissBeat = true;
		AudioManager.Instance.Play("PlayerMissedBeat");

		yield return new WaitForSeconds(_penaltyCooldown);

		_didPlayerMissBeat = false;
	}

	private IEnumerator PlayAttack()
	{
		_isAttackCoroutineRunning = true;

		ChordClip currentChordClip = _playerChords.CurrentChordSet.chordClips[_currentChord];
		currentChordClip.source.Play();

		bool isChordPlaying = currentChordClip.clip != null;
		SetAttackComponents(isChordPlaying);

		if (isChordPlaying) InitializeProximityAttack();

		CheckIfSongDone();

		yield return new WaitForSeconds(_playerChords.CurrentChordSet.time);

		SetAttackComponents(false);

		_isAttackCoroutineRunning = false;

		if (Input.GetButton("Fire1") && _currentChord != 0) //continue chord progression
			StartCoroutine(PlayAttack());
	}

	private void SetAttackComponents(bool value)
	{
		_attackObject.SetActive(value);
		_animator.SetBool("Attack", value);
	}

	private void InitializeProximityAttack()
	{
		ChordSetSO currentChordSet = _playerChords.CurrentChordSet;

		_attackObjectController.MaxScale = _attackObjectController.AnimationSpeed * currentChordSet.time;
		_attackObjectController.AnimationSpeedMultiplier = currentChordSet.chordClips[_currentChord].beats;
		_attackObjectController.HitboxScaleResetCounter = currentChordSet.chordClips[_currentChord].beats;
	}

	private void CheckIfSongDone()
	{
		bool isSongDone = _currentChord >= (_playerChords.CurrentChordSet.chordClips.Length - 1);
		if (!isSongDone)
			_currentChord++;
		else
			_currentChord = 0;
	}
}
