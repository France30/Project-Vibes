using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Button Event", menuName = "UI/Button Events")]
public class ButtonEvent : ScriptableObject
{
    public void NewGame()
    {
        SaveSystem.ClearAllSaveData();
        LevelManager.Instance.LoadLevelSelect(1);
        AudioManager.Instance.Stop("MainMenuBGM");
    }

    public void ResetLevel()
    {
        LevelManager.Instance.ResetLevel();
    }
}
