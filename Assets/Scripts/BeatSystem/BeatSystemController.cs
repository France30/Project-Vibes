using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSystemController : Singleton<BeatSystemController>
{
    [SerializeField] private float _beatTime = 4.0f;
    [SerializeField] private float _beatSpeed = 1.0f;

    [SerializeField] private ImageController _tickUI;
    [SerializeField] private ImageController _beatUI;

    private float _currentTime = 0;

    public bool IsBeatPlaying { get; set; }

    private void Start()
    {
        IsBeatPlaying = false;

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
