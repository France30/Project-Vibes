using System.Collections;
using UnityEngine;

public class BeatSystemController : Singleton<BeatSystemController>
{
    [SerializeField] private int _beatCount = 4;
    [SerializeField] private float _beatSpeed = 1.0f;

    [SerializeField] private BeatSystemUI _tickUI;
    [SerializeField] private BeatSystemUI _beatUI;

    private float _currentCount = 0;
    private WaitForSeconds _waitForBeatSpeed;
    private bool _isBeatPlaying = false;

    public bool IsBeatPlaying { get { return _isBeatPlaying; } }


    private void Start()
    {
        _waitForBeatSpeed = new WaitForSeconds(_beatSpeed);

        StartCoroutine(BeatSystem());
    }

    private IEnumerator BeatSystem()
    {
        yield return new WaitForSeconds(_beatSpeed);

        _currentCount++;
        if (_currentCount < _beatCount)
            PlayTick();
        else
            PlayBeat();

        StartCoroutine(BeatSystem());
    }

    private void PlayTick()
    {
        _tickUI.ImageAlpha = 0.5f;
        _beatUI.ImageAlpha = 0f;

        AudioManager.Instance.Play("TickBGM");       

        _isBeatPlaying = false;
    }

    private void PlayBeat()
    {
        _beatUI.ImageAlpha = 0.5f;
        _tickUI.ImageAlpha = 0f;

        AudioManager.Instance.Play("BeatBGM");
        _currentCount = 0;

        _isBeatPlaying = true;
    }
}
