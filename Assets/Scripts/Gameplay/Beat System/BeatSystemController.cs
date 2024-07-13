using System.Collections;
using UnityEngine;

public class BeatSystemController : Singleton<BeatSystemController>
{
	[SerializeField] private Beat[] _beats;
	[SerializeField] private BeatSystemUI _tickUI;
	[SerializeField] private BeatSystemUI _beatUI;

	private int _currentBeat = 0;
	private int _currentCount = 0;
	//private WaitForSeconds _waitForBeatSpeed;
	private bool _isBeatPlaying = false;
	private float _bpm = 0f;

	public bool IsBeatPlaying { get { return _isBeatPlaying; } }
	public int TickCount { get; private set; }
	public bool IsBeatUIEnabled { get; private set; }


	public void CalculateBPMWithMovement(Vector2 velocity)
	{
		float horizontalVelocity = Calculate.RoundedAbsoluteValue(velocity.x);
		float verticalVelocity = Calculate.RoundedAbsoluteValue(velocity.y);
		//Debug.Log("horizontal velocity: " + horizontalVelocity + " vertical velocity: " + verticalVelocity);

		float moveSpeed = (horizontalVelocity >= verticalVelocity) ? horizontalVelocity : verticalVelocity;
		moveSpeed = Mathf.Clamp(moveSpeed, 0f, 1f);
		//Debug.Log(moveSpeed);
		_bpm = _beats[_currentBeat].beatSpeed / (1f + moveSpeed);
		if(_bpm <= 0f)
		{
			_bpm = 0.1f;
		}
	}

	public void EnableBeatUI(bool enable)
    {
		_tickUI.gameObject.SetActive(enable);
		_beatUI.gameObject.SetActive(enable);

		IsBeatUIEnabled = enable;
    }

	private void OnEnable()
	{
		//_waitForBeatSpeed = new WaitForSeconds(_beats[_currentBeat].beatSpeed);

		StartCoroutine(BeatSystem());
	}

	private void OnDisable()
    {
		StopCoroutine(BeatSystem());
		_currentBeat = 0;
		_currentCount = 0;
		_isBeatPlaying = false;
		//_bpm = 0f;
	}

	private IEnumerator BeatSystem()
	{
		float offset = (IsBeatPlaying) ? _beats[_currentBeat].openingOffset : 0;
		yield return new WaitForSeconds(_beats[_currentBeat].beatSpeed + offset);

		_currentCount++;
		if (_currentCount < _beats[_currentBeat].beatCount)
			PlayTick();
		else
			PlayBeat();

		StartCoroutine(BeatSystem());
	}

	private void PlayTick()
	{
		TickCount = _currentCount;
		_isBeatPlaying = false;

		_tickUI.ImageAlpha = 0.5f;
		_beatUI.ImageAlpha = 0f;

		AudioManager.Instance.Play(_beats[_currentBeat].tickBGM);
	}

	private void PlayBeat()
	{
		_currentCount = 0;
		TickCount = _currentCount;

		_isBeatPlaying = true;

		_currentBeat = (_currentBeat < _beats.Length - 1) ? _currentBeat + 1 : 0;

		_beatUI.ImageAlpha = 1f;
		_tickUI.ImageAlpha = 0f;

		AudioManager.Instance.Play(_beats[_currentBeat].beatBGM);
	}
}
