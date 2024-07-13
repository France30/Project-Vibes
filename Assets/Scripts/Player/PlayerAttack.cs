using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
	[SerializeField] private GameObject _attackObject;
	[SerializeField] private PlayerChords _playerChords;
	[SerializeField] private float _penaltyCooldown = 3.0f;
	[SerializeField] private PlayerAnimatorController _animator;
	[SerializeField] private float _nextSongComboTime = 3f;

	[Header("Cooldown Indicator UI")]
	[SerializeField] private Image _coolDownIndicator;
	[SerializeField] private Sprite _baseSprite;
	[SerializeField] private Sprite _coolDownSprite;
	//[SerializeField] private Image _playingIndicator;

	private AttackObjectController _attackObjectController;

	private bool _isAttackCoroutineRunning = false;
	private bool _didPlayerMissBeat = false;
	private int _currentChord = 0;

	private float _remainingComboTime;

	public PlayerChords PlayerChords { get { return _playerChords; } }

	private void Awake()
	{
		_attackObjectController = _attackObject.GetComponent<AttackObjectController>();
		//_playingIndicator.enabled = false;
		BeatSystemController.Instance.EnableBeatUI(false);

		if (_attackObject.activeSelf)
			_attackObject.SetActive(false);
	}

	private void OnDisable()
	{
		ResetCurrentChordSet();
	}

	// Update is called once per frame
	private void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if (_didPlayerMissBeat) return;

			if (!_isAttackCoroutineRunning && !BeatSystemController.Instance.IsBeatUIEnabled)
			{
				//_playingIndicator.enabled = true;
				BeatSystemController.Instance.EnableBeatUI(true);
				StartCoroutine(PlayAttack());
			}
		}
	}

	private IEnumerator AttackNotOnBeat()
	{
		_didPlayerMissBeat = true;
		ResetCurrentChordSet();
		AudioManager.Instance.Play("PlayerMissedBeat");
		UpdateCooldownIndicatorUI(BeatCooldown.MissedBeat);

		yield return new WaitForSeconds(_penaltyCooldown);

		UpdateCooldownIndicatorUI(BeatCooldown.Base);
		AudioManager.Instance.Play("AttackReadySFX");
		_didPlayerMissBeat = false;
	}

	private void UpdateCooldownIndicatorUI(BeatCooldown beatCooldown)
	{
		switch (beatCooldown)
		{
			case BeatCooldown.Base:
				_coolDownIndicator.sprite = _baseSprite;
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

		ChordClip nextChordClip = _playerChords.CurrentChordSet.chordClips[_currentChord];

		yield return new WaitForSeconds(_playerChords.CurrentChordSet.time);

		SetAttackComponents(false);

		_isAttackCoroutineRunning = false;

		if (!nextChordClip.isStartOfNextSheet) //continue chord progression
		{
			StartCoroutine(PlayAttack());
			yield break;
		}

		if(_currentChord != 0)
			StartCoroutine(NextSheetCombo(nextChordClip));
		else
			ResetCurrentChordSet();
	}

	private IEnumerator NextSheetCombo(ChordClip nextSheetClip)
    {
		_remainingComboTime = _nextSongComboTime;
		while (_remainingComboTime > 0)
        {
			_remainingComboTime -= Time.deltaTime;

			if (Input.GetButtonDown("Fire1") && nextSheetClip.isStartOfNextSheet && BeatSystemController.Instance.IsBeatPlaying)
			{
				StartCoroutine(PlayAttack());
				yield break;
			}
			else if (Input.GetButtonDown("Fire1") && !BeatSystemController.Instance.IsBeatPlaying)
			{
				StartCoroutine(AttackNotOnBeat());
				yield break;
			}

			yield return null;
        }

		ResetCurrentChordSet();
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

	private void ResetCurrentChordSet()
	{
		_currentChord = 0;
		if(BeatSystemController.Instance != null)
			BeatSystemController.Instance.EnableBeatUI(false);
	}
}
