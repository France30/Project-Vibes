using UnityEngine;

[CreateAssetMenu(fileName = "New Chord", menuName = "Chord")]
public class Chord : ScriptableObject
{
    public float time = 1f;
    public ChordClip[] chordClips;

    [HideInInspector]
    public WaitForSeconds waitForTime;
}
