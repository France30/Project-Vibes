using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ChordSet : MonoBehaviour
{
    [SerializeField] private ChordSetSO _chordSO;

    public ChordType ChordType { get { return _chordSO.chordType; } }
    public ChordClip[] ChordClips { get { return _chordSO.chordClips; } }
    public float ChordTime { get { return _chordSO.time; } }


    public void AddChordClips(ChordClip[] otherChordClips)
    {
        ChordClip[] chordClips = _chordSO.chordClips.Concat<ChordClip>(otherChordClips).ToArray();
        _chordSO.chordClips = chordClips;

        InitializeChordAudioSource(_chordSO);
    }

    private void Awake()
    {
        InitializeChordAudioSource(_chordSO);
    }

    private void InitializeChordAudioSource(ChordSetSO chordSO)
    {
        ChordClip[] chordClips = chordSO.chordClips;
        for (int i = 0; i < chordClips.Length; i++)
        {
            if (chordClips[i].source != null) continue;

            chordClips[i].source = gameObject.AddComponent<AudioSource>();
            chordClips[i].source.clip = chordClips[i].clip;

            chordClips[i].source.volume = chordClips[i].volume;
            chordClips[i].source.pitch = chordClips[i].pitch;
        }
    }
}
