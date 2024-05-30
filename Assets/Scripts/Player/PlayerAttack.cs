using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
	[SerializeField] private GameObject _attackObject;
	[SerializeField] private PlayerChords _playerChords;
	[SerializeField] private float _penaltyCooldown = 3.0f;
	[SerializeField] private PlayerAnimatorController _animator;

	[Header("Cooldown Indicator UI")]
	[SerializeField] private Image _coolDownIndicator;
	[SerializeField] private Sprite _coolDownSprite;
	[SerializeField] private Sprite _tickSprite;
	[SerializeField] private Sprite _beatSprite;

	private AttackObjectController _attackObjectController;

	private bool _isAttackCoroutineRunning = false;
	private bool _didPlayerMissBeat = false;
	private int _currentChord = 0;

	private WaitForSeconds _waitForPenaltyCooldown;

	public PlayerChords PlayerChords { get { return _playerChords; } }

	private void Awake()
	{
		_attackObjectController = _attackObject.GetComponent<AttackObjectController>();

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
		if(!_didPlayerMissBeat && !_isAttackCoroutineRunning)
        {
			if (BeatSystemController.Instance.IsBeatPlaying)
				UpdateCooldownIndicatorUI(BeatCooldown.Beat);
			else
				UpdateCooldownIndicatorUI(BeatCooldown.Tick);
		}

		if (Input.GetButtonDown("Fire1"))
		{
			if (_didPlayerMissBeat) return;

			if(!BeatSystemController.Instance.IsBeatPlaying)
			{
				StartCoroutine(AttackNotOnBeat());
				return;
			}

			if (!_isAttackCoroutineRunning)
			{
				UpdateCooldownIndicatorUI(BeatCooldown.Beat);
				StartCoroutine(PlayAttack());
			}
		}

		if (Input.GetButtonUp("Fire1"))
			_currentChord = 0;
	}

	private IEnumerator AttackNotOnBeat()
	{
		_didPlayerMissBeat = true;
		AudioManager.Instance.Play("PlayerMissedBeat");
		UpdateCooldownIndicatorUI(BeatCooldown.MissedBeat);

		yield return new WaitForSeconds(_penaltyCooldown);

		AudioManager.Instance.Play("AttackReadySFX");
		_didPlayerMissBeat = false;
	}

	private void UpdateCooldownIndicatorUI(BeatCooldown beatCooldown)
	{
		switch (beatCooldown)
		{
			case BeatCooldown.Tick:
				_coolDownIndicator.sprite = _tickSprite;
				break;
			case BeatCooldown.Beat:
				_coolDownIndicator.sprite = _beatSprite;
				break;
			case BeatCooldown.MissedBeat:
				_coolDownIndicator.sprite = _coolDownSprite;
				break;
		}
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
		_animator.SetAttackParam(value);
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
