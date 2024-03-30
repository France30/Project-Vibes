using UnityEngine;
using UnityEditor;

public enum ChordType
{
    ForestChords,
    CaveChords,
    CastleChords,
    SpecialEvent
}

[CreateAssetMenu(fileName = "New Chord Set SO", menuName = "ChordSystem/ChordSetSO")]
public class ChordSetSO : ScriptableObject
{
    [HideInInspector] public WaitForSeconds waitForTime;

    [SerializeField] private bool _isDrop = false;
    [SerializeField] private string _id = "0";
    [SerializeField] private ChordType _chordType;

    public float time = 1f;
    public ChordClip[] chordClips;

    private ObjectWithPersistentData _chordSetDrop = ObjectWithPersistentData.chordSetDrop;

    //default values
    private ChordClip[] _chordClips;
    private bool _wasDrop = false;

    public bool IsDrop { get { return _isDrop; } }
    public ChordType ChordType { get { return _chordType; } }
    public string ID { get { return _id; } }


    public void DropGet()
    {
        if (!_isDrop) return;

        _isDrop = false;
        SavePersistentData.SavePersistentFlag(_chordSetDrop, _id, _isDrop);
    }

    public void ResetToDefault()
    {
        chordClips = _chordClips;
        _isDrop = _wasDrop;
    }

    private void OnEnable()
    {
        _chordClips = chordClips;
        _wasDrop = _isDrop;

#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += ResetToDefaultValues;
#endif
        if (!_isDrop) return;

        _isDrop = SavePersistentData.LoadPersistentFlag(_chordSetDrop, _id);
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
        SavePersistentData.ClearPersistentFlagData(_chordSetDrop, _id);
        _isDrop = _wasDrop;
    }
#endif
}
