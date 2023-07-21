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

    public delegate void LevelLoad();
    public event LevelLoad OnLevelLoad;

    public List<int> LevelsUnlocked { get { return _levelsUnlocked; } }

    
    public void AddLevel(int level)
    {
        if (!_levelsUnlocked.Contains(level))
        {
            _levelsUnlocked.Add(level);
            SaveSystem.SaveUnlockedLevels();
        }
    }

    public void LoadLevelSelect(int sceneIndex)
    {
        if (!_isLoadingLevel)
        {
            _isLoadingLevel = true;
            StartCoroutine(LoadLevel(sceneIndex));
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

    public void LoadLevelFromSave()
    {
        if (!_isLoadingLevel)
        {
            _isLoadingLevel = true;
            StartCoroutine(LoadSavedLevel());
        }
    }

    public void ResetLevel()
    {
        if (!_isLoadingLevel)
        {
            _isLoadingLevel = true;
            StartCoroutine(RestartLevel());
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _loadingScreen.SetActive(false);
        _isPersist = true;

        _levelsUnlocked = SaveSystem.LoadUnlockedLevels();
    }

    private IEnumerator LoadSavedLevel()
    {
        PlayerData playerData = SaveSystem.LoadPlayerData();

        yield return StartCoroutine(LoadLevel(playerData.currentLevelSelect));

        LoadPlayerPositionInLevel(playerData);
    }

    private IEnumerator RestartLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        yield return StartCoroutine(LoadLevel(currentSceneIndex));

        PlayerData playerData = SaveSystem.LoadPlayerData();
        LoadPlayerPositionInLevel(playerData);
    }

    private IEnumerator LevelTransition(int sceneIndex, string spawnArea, bool saveOnTransition = false)
    {
        yield return StartCoroutine(LoadLevel(sceneIndex));

        SetPlayerPosition(spawnArea, saveOnTransition);
    }

    private IEnumerator LoadLevel(int sceneIndex)
    {
        OnLevelLoad?.Invoke();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        yield return StartCoroutine(LoadingScreen(operation));

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

    private void LoadPlayerPositionInLevel(PlayerData playerData)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (playerData == null || playerData.currentLevelSelect != currentSceneIndex) return;
        
        Vector2 playerPosition = new Vector2(playerData.playerPosition[0], playerData.playerPosition[1]);
        if (playerPosition != Vector2.zero)
        {
            GameController.Instance.Player.transform.position = playerPosition;
        }
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
