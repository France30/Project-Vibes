using UnityEngine;

[CreateAssetMenu(fileName = "New Button Event", menuName = "UI/Button Events")]
public class ButtonEvent : ScriptableObject
{
	public void NewGame()
	{
		if (SaveSystem.IsSaveFileFound())
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
		}

		if(SaveSystem.IsSaveFileFound() && LevelManager.Instance.CurrentLevel > 1)
			LevelManager.Instance.LoadLevelSelect(1); //load first level if save file exists
		else
			LevelManager.Instance.UnloadLevelScene(0); //we unload the main menu if we are already in the first level

		SaveSystem.ClearAllSaveData();
		LevelManager.Instance.LevelsUnlocked.Clear();
		LevelManager.Instance.AddLevel(1);
		GameController.Instance.DisableGameControls(false);
	}

	public void ContinueGame()
	{
		//LevelManager.Instance.LoadLevelFromSave();
		LevelManager.Instance.UnloadLevelScene(0);
		GameController.Instance.DisableGameControls(false);
	}

	public void ResetLevel()
	{
		LevelManager.Instance.ResetLevel();
	}

	public void ReturnToMainMenu()
	{
		GameController.Instance.ResetGameControllerConfigs();

		LevelManager.Instance.LoadLevelSelectAdditively(0);

		//load last saved checkpoint in level
		PlayerData playerData = SaveSystem.LoadPlayerData();
		LevelManager.Instance.LoadPlayerPositionInLevel(playerData);
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
