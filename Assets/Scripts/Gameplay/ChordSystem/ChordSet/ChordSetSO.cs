using UnityEngine;
using UnityEditor;

public enum ChordType
{
    Chord1 //test
}

[CreateAssetMenu(fileName = "New Chord Set SO", menuName = "ChordSystem/ChordSetSO")]
public class ChordSetSO : ScriptableObject
{
    public ChordType chordType;
    public float time = 1f;
    public ChordClip[] chordClips;

    [HideInInspector]
    public WaitForSeconds waitForTime;

    //default values
    private ChordClip[] _chordClips;


    private void OnEnable()
    {
#if UNITY_EDITOR
        _chordClips = chordClips;
        EditorApplication.playModeStateChanged += ResetToDefaultValues;
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged -= ResetToDefaultValues;
#endif
    }

#if UNITY_EDITOR
    private void ResetToDefaultValues(PlayModeStateChange playModeStateChange)
    {
        if (playModeStateChange != PlayModeStateChange.ExitingPlayMode) return;

        chordClips = _chordClips;
    }
#endif
}
