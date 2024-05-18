using UnityEngine;

[CreateAssetMenu(fileName = "New Button Event", menuName = "UI/Button Events")]
public class ButtonEvent : ScriptableObject
{
	public void NewGame()
	{
		ChordSetSO[] chordSetSOs = Resources.LoadAll<ChordSetSO>("Scriptable Objects/ChordSetSO");
		for (int i = 0; i < chordSetSOs.Length; i++)
		{
			chordSetSOs[i].ResetToDefault();
		}

		MusicSheetSO[] musicSheetSO = Resources.LoadAll<MusicSheetSO>("Scriptable Objects/MusicSheetSO");
		for (int i = 0; i < musicSheetSO.Length; i++)
		{
			musicSheetSO[i].Reset();
		}

		SaveSystem.ClearAllSaveData();
		LevelManager.Instance.LevelsUnlocked.Clear();
		LevelManager.Instance.AddLevel(1);

		LevelManager.Instance.LoadLevelSelect(1);
	}

	public void ContinueGame()
	{
		LevelManager.Instance.UnloadLevelScene(0);
		GameController.Instance.DisableGameControls(false);
	}

	public void ResetLevel()
	{
		LevelManager.Instance.ResetLevel(false);
	}

	public void RetryLastCheckpoint()
    {
		LevelManager.Instance.LoadLevelFromSave(false);
	}

	public void ReturnToMainMenu()
	{
		GameController.Instance.ResetGameControllerConfigs();

		//load last saved checkpoint
		PlayerData playerData = SaveSystem.LoadPlayerData();
		if (playerData != null)
			LevelManager.Instance.LoadLevelFromSave(true);
		else
        {
			LevelManager.Instance.ResetLevel(true);
		}
	}

	public void LevelSelect(int level)
	{
		SaveSystem.ClearSavedPlayerPositionInLevel(level);
		SaveSystem.ClearCheckpointData();
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
