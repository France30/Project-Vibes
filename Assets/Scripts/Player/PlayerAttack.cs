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

	[Header("Music Player UI")]
	[SerializeField] private Image _musicPlayer;
	[SerializeField] private Sprite _inactiveMusicSheet;
	[SerializeField] private Sprite _playingMusicSheet;
	[SerializeField] private float _musicPlayerFadeSpeed = 1f;
	[SerializeField] private float _musicPlayerVisibleDuration = 5f;

	private AttackObjectController _attackObjectController;

	private bool _isAttackCoroutineRunning = false;
	private bool _didPlayerMissBeat = false;
	private int _currentChord = 0;

	private float _remainingMusicPlayerDuration;
	private float _remainingComboTime;
	private int _currentCombo = 0;

	public delegate void OnHUDFadeEvent(float alpha);
	public event OnHUDFadeEvent OnHUDFade;
	public PlayerChords PlayerChords { get { return _playerChords; } }


	public void RefreshMusicPlayerUI()
	{
		var musicPlayerSheets = _musicPlayer.transform.childCount;
		for (int i = 0; i < musicPlayerSheets; i++)
		{
			var musicSheet = _musicPlayer.transform.GetChild(i);
			musicSheet.gameObject.SetActive(false);
			musicSheet.GetComponent<Image>().sprite = _inactiveMusicSheet;
		}
	}

	public void UpdateMusicPlayerUI(ChordSet chordSet)
	{
		//set initial sheet as active
		_musicPlayer.transform.GetChild(0).gameObject.SetActive(true);

		var musicSheets = chordSet.MusicSheets;
		for (int i = 0; i < musicSheets.Length; i++)
		{
			if (musicSheets[i].isFound)
			{
				var musicSheet = _musicPlayer.transform.GetChild(i + 1);
				musicSheet.gameObject.SetActive(true);
			}
			else
				_musicPlayer.transform.GetChild(i + 1).gameObject.SetActive(false);
		}
	}

	private void Awake()
	{
		_attackObjectController = _attackObject.GetComponent<AttackObjectController>();
		//_playingIndicator.enabled = false;
		BeatSystemController.Instance.EnableBeatUI(false);

		if (_attackObject.activeSelf)
			_attackObject.SetActive(false);
	}

	private void Start()
	{
		OnHUDFade += (float alpha) => { _musicPlayer.color = new Color(_musicPlayer.color.r, _musicPlayer.color.g, _musicPlayer.color.b, alpha); };

		int musicPlayerSheets = _musicPlayer.transform.childCount;
		for(int i = 0; i < musicPlayerSheets; i++)
        {
			var sheet = _musicPlayer.transform.GetChild(i).GetComponent<Image>();
			OnHUDFade += (float alpha) => {sheet.color = new Color(sheet.color.r, sheet.color.g, sheet.color.b, alpha); };
		}

		OnHUDFade?.Invoke(0);
	}

    private void OnDisable()
	{
		if ((GameController.Instance != null && GameController.Instance.Player.CurrentHealth <= 0) || Time.timeScale > 0)
		{
			StopAllCoroutines();
			ResetCurrentChordSet();
		}
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
				_isAttackCoroutineRunning = true;
				_remainingMusicPlayerDuration = _musicPlayerVisibleDuration;

				BeatSystemController.Instance.EnableBeatUI(true);
				_musicPlayer.transform.GetChild(0).GetComponent<Image>().sprite = _playingMusicSheet;

				StartCoroutine(PlayAttack());
			}
		}

		if (_isAttackCoroutineRunning && _musicPlayer.color.a < 1)
			FadeInMusicPlayerUI();

		if(!_isAttackCoroutineRunning && _musicPlayer.color.a > 0)
        {
			_remainingMusicPlayerDuration -= Time.deltaTime;
			if (_remainingMusicPlayerDuration <= 0 || (_musicPlayer.color.a < 1 && _musicPlayer.color.a > 0))
				FadeOutMusicPlayerUI();
        }
	}

	private void FadeOutMusicPlayerUI()
	{
		float alpha = _musicPlayer.color.a - _musicPlayerFadeSpeed * Time.deltaTime;
		OnHUDFade?.Invoke(alpha);
	}

	private void FadeInMusicPlayerUI()
	{
		float alpha = _musicPlayer.color.a + _musicPlayerFadeSpeed * Time.deltaTime;
		OnHUDFade?.Invoke(alpha);
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
		ChordClip currentChordClip = _playerChords.CurrentChordSetSO.chordClips[_currentChord];
		currentChordClip.source.Play();

		bool isChordPlaying = currentChordClip.clip != null;
		SetAttackComponents(isChordPlaying);

		if (isChordPlaying) InitializeProximityAttack();

		CheckIfSongDone();

		ChordClip nextChordClip = _playerChords.CurrentChordSetSO.chordClips[_currentChord];

		yield return new WaitForSeconds(_playerChords.CurrentChordSetSO.time);

		SetAttackComponents(false);

		if (!nextChordClip.isStartOfNextSheet) //continue chord progression
		{
			StartCoroutine(PlayAttack());
			yield break;
		}

		StartCoroutine(NextSheetCombo(nextChordClip));
	}

	private IEnumerator NextSheetCombo(ChordClip nextSheetClip)
    {
		_remainingComboTime = _nextSongComboTime;
		while (_remainingComboTime > 0)
        {
			_remainingComboTime -= Time.deltaTime;

			if (Input.GetButtonDown("Fire1") && nextSheetClip.isStartOfNextSheet && BeatSystemController.Instance.IsBeatPlaying)
			{
				_musicPlayer.transform.GetChild(_currentCombo).GetComponent<Image>().sprite = _inactiveMusicSheet;
				_currentCombo = (_currentChord > 0) ? _currentCombo + 1 : 0;
				_musicPlayer.transform.GetChild(_currentCombo).GetComponent<Image>().sprite = _playingMusicSheet;

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
		ChordSetSO currentChordSet = _playerChords.CurrentChordSetSO;

		_attackObjectController.MaxScale = _attackObjectController.AnimationSpeed * currentChordSet.time;
		_attackObjectController.AnimationSpeedMultiplier = currentChordSet.chordClips[_currentChord].beats;
		_attackObjectController.HitboxScaleResetCounter = currentChordSet.chordClips[_currentChord].beats;
	}

	private void CheckIfSongDone()
	{
		bool isSongDone = _currentChord >= (_playerChords.CurrentChordSetSO.chordClips.Length - 1);
		if (!isSongDone)
			_currentChord++;
		else
			_currentChord = 0;
	}

	private void ResetCurrentChordSet()
	{
		_isAttackCoroutineRunning = false;

		_currentChord = 0;
		_currentCombo = 0;

		RefreshMusicPlayerUI();
		UpdateMusicPlayerUI(_playerChords.CurrentChordSet);

		SetAttackComponents(false);

		if (BeatSystemController.Instance != null)
			BeatSystemController.Instance.EnableBeatUI(false);
	}
}
