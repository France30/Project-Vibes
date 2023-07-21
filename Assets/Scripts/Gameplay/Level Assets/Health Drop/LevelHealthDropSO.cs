using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Level Health Drop SO", menuName = "Interactables/LevelHealthDropSO")]
public class LevelHealthDropSO : ScriptableObject
{
    [SerializeField] private string _id = "0";
    [SerializeField] private int _amountToRestore = 1;

    private ObjectWithPersistentData _levelHealthDrop = ObjectWithPersistentData.levelHealthDrop;
    private bool _isDropInScene = true;

    public bool IsDropInScene { get { return _isDropInScene; } }
    public int AmountToRestore { get { return _amountToRestore; } }


    public void DropGet()
    {
        _isDropInScene = false;
        SavePersistentData.SavePersistentFlag(_levelHealthDrop, _id, _isDropInScene);
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += ResetFlag;
#endif

        _isDropInScene = SavePersistentData.LoadPersistentFlag(_levelHealthDrop, _id);
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

        SavePersistentData.ClearPersistentFlagData(_levelHealthDrop, _id);
    }
#endif
}
