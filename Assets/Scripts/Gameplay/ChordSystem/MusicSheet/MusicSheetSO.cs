using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Music Sheet SO", menuName = "ChordSystem/MusicSheetSO")]
public class MusicSheetSO : ScriptableObject
{
    public ChordType chordType;
    public ChordClip[] chordClips;

    [SerializeField] private string _id = "0";

    private ObjectWithPersistentData _musicSheet = ObjectWithPersistentData.musicSheet;
    private bool _isSheetInScene = true;

    public bool IsSheetInScene { get { return _isSheetInScene; } }


    public void SaveSheet()
    {
        _isSheetInScene = false;
        SavePersistentData.SavePersistentFlag(_musicSheet, _id, _isSheetInScene);
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += ResetFlag;
#endif

        _isSheetInScene = SavePersistentData.LoadPersistentFlag(_musicSheet, _id);
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged -= ResetFlag;
#endif
    }

#if UNITY_EDITOR
    private void ResetFlag(PlayModeStateChange playModeStateChange)
    {
        if (playModeStateChange != PlayModeStateChange.ExitingPlayMode) return;

        SavePersistentData.ClearPersistentFlagData(_musicSheet, _id);
    }
#endif
}
