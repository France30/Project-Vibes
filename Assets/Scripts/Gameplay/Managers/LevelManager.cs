using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : Singleton<LevelManager>
{
	[SerializeField] private GameObject _loadingScreen;
	[SerializeField] private Slider _loadingBar;
	[SerializeField] private TextMeshProUGUI _loadingText;

	private bool _isLoadingLevel = false;
	private List<int> _levelsUnlocked = new List<int>();
	private int _currentLevel = 1;

	public delegate void LevelLoad();
	public event LevelLoad OnLevelLoad;
	public event LevelLoad OnLoadFromSave;

	public List<int> LevelsUnlocked { get { return _levelsUnlocked; } }
	public int CurrentLevel { get { return _currentLevel; } }

	
	public void AddLevel(int level)
	{
		if (!_levelsUnlocked.Contains(level))
		{
			_levelsUnlocked.Add(level);
			SaveSystem.SaveUnlockedLevels();
		}
	}

	public bool CheckLevelActive()
    {
		return SceneManager.sceneCount > 1;
    }

	public void LoadLevelSelect(int sceneIndex, bool withMainMenu = false)
	{
		if (!_isLoadingLevel)
		{
			_isLoadingLevel = true;
			StartCoroutine(LoadLevel(sceneIndex, withMainMenu));
		}
	}

	public void LoadLevelSelectAdditively(int sceneIndex)
    {
		if (!_isLoadingLevel)
		{
			_isLoadingLevel = true;
			StartCoroutine(LoadLevelAdditively(sceneIndex));
		}
	}

	public void LoadLevelTransition(int sceneIndex, string spawnArea, bool saveOnTransition = false)
	{
		if (!_isLoadingLevel)
		{
			_isLoadingLevel = true;
			StartCoroutine(LevelTransition(sceneIndex, spawnArea, saveOnTransition));
		}
	}

	public void LoadLevelFromSaveAdditively()
	{
		if (!_isLoadingLevel)
		{
			_isLoadingLevel = true;
			StartCoroutine(LoadSavedLevelAdditively());
		}
	}

	public void LoadLevelFromSave(bool withMainMenu)
	{
		if (!_isLoadingLevel)
		{
			_isLoadingLevel = true;
			StartCoroutine(LoadSavedLevel(withMainMenu));
		}
	}

	public void ResetLevel(bool withMainMenu)
	{
		if (!_isLoadingLevel)
		{
			_isLoadingLevel = true;
			StartCoroutine(RestartLevel(withMainMenu));
		}
	}

	public void UnloadLevelScene(int sceneIndex)
    {
		SceneManager.UnloadSceneAsync(sceneIndex);
    }

	public void LoadPlayerPositionInLevel(PlayerData playerData)
	{
		if (playerData == null || playerData.currentLevelSelect != _currentLevel) return;

		Vector2 playerPosition = new Vector2(playerData.playerPosition[0], playerData.playerPosition[1]);
		if (playerPosition != Vector2.zero)
		{
			OnLoadFromSave?.Invoke();
			GameController.Instance.Player.transform.position = playerPosition;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		_loadingScreen.SetActive(false);
		_isPersist = true;

		_levelsUnlocked = SaveSystem.LoadUnlockedLevels();
	}

	//use this to reload last checkpoint
	private IEnumerator LoadSavedLevel(bool withMainMenu)
	{
		PlayerData playerData = SaveSystem.LoadPlayerData();

		yield return StartCoroutine(LoadLevel(playerData.currentLevelSelect, withMainMenu));

        LoadPlayerPositionInLevel(playerData);
	}

    private IEnumerator LoadSavedLevelAdditively()
	{
		PlayerData playerData = SaveSystem.LoadPlayerData();

		yield return StartCoroutine(LoadLevelAdditively(playerData.currentLevelSelect));

		LoadPlayerPositionInLevel(playerData);
	}

	private IEnumerator RestartLevel(bool withMainMenu)
	{
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

		yield return StartCoroutine(LoadLevel(currentSceneIndex, withMainMenu));

		PlayerData playerData = SaveSystem.LoadPlayerData();
		LoadPlayerPositionInLevel(playerData);
	}

	private IEnumerator LevelTransition(int sceneIndex, string spawnArea, bool saveOnTransition = false)
	{
		yield return StartCoroutine(LoadLevel(sceneIndex));

		SetPlayerPosition(spawnArea, saveOnTransition);
	}

	private IEnumerator LoadLevel(int sceneIndex, bool withMainMenu = false)
	{
		if (sceneIndex > 0)
			_currentLevel = sceneIndex;

		OnLevelLoad?.Invoke();
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
		yield return StartCoroutine(LoadingScreen(operation));

		GameController.Instance.DisableGameControls(withMainMenu);
		Time.timeScale = 1;
		_isLoadingLevel = false;

		if (withMainMenu)
			LevelManager.Instance.LoadLevelSelectAdditively(0);
	}

	//used for dynamic real-time main menu
	private IEnumerator LoadLevelAdditively(int sceneIndex)
	{
		if (sceneIndex > 0)
			_currentLevel = sceneIndex;

		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
		yield return StartCoroutine(LoadingScreen(operation));

		GameController.Instance.DisableGameControls(true);
		Time.timeScale = 1;
		_isLoadingLevel = false;
	}

	private IEnumerator LoadingScreen(AsyncOperation operation)
	{
		_loadingScreen.SetActive(true);

		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / .9f);
			_loadingBar.value = progress;
			_loadingText.text = progress * 100f + "%";
			yield return null;
		}

		_loadingScreen.SetActive(false);
	}

	private void SetPlayerPosition(string spawnPosition, bool savePlayerPosition = false)
	{
		PlayerSpawnArea[] spawnAreas = FindObjectsOfType<PlayerSpawnArea>();
		for (int i = 0; i < spawnAreas.Length; i++)
		{
			if (spawnAreas[i].ID != spawnPosition) continue;

			GameController.Instance.Player.transform.position = spawnAreas[i].transform.position;
			if(savePlayerPosition)
			{
				SaveSystem.SavePlayerData();
			}
			break;
		}
	}
}
