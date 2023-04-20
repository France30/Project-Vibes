using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Chord", menuName = "Chord")]
public class Chord : ScriptableObject
{
    public float time = 1f;
    public ChordClip[] chordClips;
}
