using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chord", menuName = "Chord")]
public class Chord : ScriptableObject
{
    public float time = 1f;
    public List<bool> playNote;
}
