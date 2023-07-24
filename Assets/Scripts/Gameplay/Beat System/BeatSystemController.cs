using System.Collections;
using UnityEngine;

public class BeatSystemController : Singleton<BeatSystemController>
{
    [SerializeField] private Beat[] _beats;
    [SerializeField] private BeatSystemUI _tickUI;
    [SerializeField] private BeatSystemUI _beatUI;

    private int _currentBeat = 0;
    private int _currentCount = 0;
    private WaitForSeconds _waitForBeatSpeed;
    private bool _isBeatPlaying = false;

    public bool IsBeatPlaying { get { return _isBeatPlaying; } }
    public int TickCount { get; private set; }


    private void Start()
    {
        _waitForBeatSpeed = new WaitForSeconds(_beats[_currentBeat].beatSpeed);

        StartCoroutine(BeatSystem());
    }

    private IEnumerator BeatSystem()
    {
        yield return new WaitForSeconds(_beats[_currentBeat].beatSpeed);

        _currentCount++;
        if (_currentCount < _beats[_currentBeat].beatCount)
            PlayTick();
        else
            PlayBeat();

        StartCoroutine(BeatSystem());
    }

    private void PlayTick()
    {
        _tickUI.ImageAlpha = 0.5f;
        _beatUI.ImageAlpha = 0f;

        AudioManager.Instance.Play(_beats[_currentBeat].tickBGM);

        TickCount = _currentCount;
        _isBeatPlaying = false;
    }

    private void PlayBeat()
    {
        _beatUI.ImageAlpha = 0.5f;
        _tickUI.ImageAlpha = 0f;

        AudioManager.Instance.Play(_beats[_currentBeat].beatBGM);

        _currentCount = 0;
        TickCount = _currentCount;

        _isBeatPlaying = true;

        _currentBeat = (_currentBeat < _beats.Length - 1) ? _currentBeat + 1 : 0;
    }
}
