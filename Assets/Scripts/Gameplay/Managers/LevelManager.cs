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

    public delegate void LevelLoad();
    public event LevelLoad OnLevelLoad;


    public void LoadLevelSelect(int sceneIndex)
    {
        if (!_isLoadingLevel)
        {
            _isLoadingLevel = true;
            StartCoroutine(LoadLevel(sceneIndex));
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

    private IEnumerator LoadLevel(int sceneIndex)
    {
        Time.timeScale = 1;

        OnLevelLoad?.Invoke();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        yield return StartCoroutine(LoadingScreen(operation));

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
        GameController.Instance.Player.transform.position = playerPosition;
    }
}
