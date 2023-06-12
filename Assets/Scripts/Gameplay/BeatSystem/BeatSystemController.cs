using System.Collections;
using UnityEngine;

public class BeatSystemController : Singleton<BeatSystemController>
{
    [SerializeField] private float _beatTime = 4.0f;
    [SerializeField] private float _beatSpeed = 1.0f;

    [SerializeField] private BeatSystemUI _tickUI;
    [SerializeField] private BeatSystemUI _beatUI;

    private float _currentTime = 0;
    private WaitForSeconds _waitForBeatSpeed;

    public bool IsBeatPlaying { get; private set; }


    private void Start()
    {
        IsBeatPlaying = false;
        _waitForBeatSpeed = new WaitForSeconds(_beatSpeed);

        StartCoroutine(BeatSystem());
    }

    private IEnumerator BeatSystem()
    {
        yield return new WaitForSeconds(_beatSpeed);

        if (_currentTime < _beatTime - 1)
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
        _currentTime ++;

        IsBeatPlaying = false;
    }

    private void PlayBeat()
    {
        _beatUI.ImageAlpha = 0.5f;
        _tickUI.ImageAlpha = 0f;

        AudioManager.Instance.Play("BeatBGM");
        _currentTime = 0;

        IsBeatPlaying = true;
    }
}
