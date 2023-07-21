using UnityEngine;

[CreateAssetMenu(fileName = "New Button Event", menuName = "UI/Button Events")]
public class ButtonEvent : ScriptableObject
{
    public void NewGame()
    {
        SaveSystem.ClearAllSaveData();

        LevelManager.Instance.LevelsUnlocked.Clear();
        LevelManager.Instance.LoadLevelSelect(1);
        LevelManager.Instance.AddLevel(1);
    }

    public void ContinueGame()
    {
        LevelManager.Instance.LoadLevelFromSave();
    }

    public void ResetLevel()
    {
        LevelManager.Instance.ResetLevel();
    }

    public void ReturnToMainMenu()
    {
        LevelManager.Instance.LoadLevelSelect(0);
    }

    public void LevelSelect(int level)
    {
        SaveSystem.ClearSavedPlayerPositionInLevel(level);
        LevelManager.Instance.LoadLevelSelect(level);
    }

    public void ExitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }
}
