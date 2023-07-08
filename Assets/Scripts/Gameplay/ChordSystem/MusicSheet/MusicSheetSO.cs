using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Music Sheet SO", menuName = "ChordSystem/MusicSheetSO")]
public class MusicSheetSO : ScriptableObject
{
    [SerializeField] private string _id = "0";
    [SerializeField] private ChordType _chordType;
    [SerializeField] private ChordClip[] _chordClips;

    private ObjectWithPersistentData _musicSheet = ObjectWithPersistentData.musicSheet;
    private bool _isSheetInScene = true;

    public bool IsSheetInScene { get { return _isSheetInScene; } }
    public ChordType ChordType { get { return _chordType; } }
    public ChordClip[] ChordClips { get { return _chordClips; } }


    public void SheetGet()
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
