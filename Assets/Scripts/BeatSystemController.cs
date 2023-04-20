using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSystemController : Singleton<BeatSystemController>
{
    [SerializeField] private float beatTime = 4.0f;
    [SerializeField] private float beatSpeed = 1.0f;

    [SerializeField] private GameObject tickUIObject;
    [SerializeField] private GameObject beatUIObject;

    private float currentTime = 0;

    public bool IsBeatPlaying { get; set; }

    private void Start()
    {
        IsBeatPlaying = false;

        StartCoroutine(beat());
    }

    private IEnumerator beat()
    {
        yield return new WaitForSeconds(beatSpeed);

        if (currentTime < beatTime - 1)
            playTick();
        else
            playBeat();

        StartCoroutine(beat());
    }

    private void playTick()
    {
        tickUIObject.GetComponent<ImageController>().ImageAlpha = 0.5f;
        beatUIObject.GetComponent<ImageController>().ImageAlpha = 0f;

        AudioManager.Instance.Play("TickBGM");
        currentTime ++;

        IsBeatPlaying = false;
    }

    private void playBeat()
    {
        beatUIObject.GetComponent<ImageController>().ImageAlpha = 0.5f;
        tickUIObject.GetComponent<ImageController>().ImageAlpha = 0f;

        AudioManager.Instance.Play("BeatBGM");
        currentTime = 0;

        IsBeatPlaying = true;
    }
}
