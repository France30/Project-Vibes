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
