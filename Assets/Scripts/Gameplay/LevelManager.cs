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

    public void ResetLevel()
    {
        if (!_isLoadingLevel)
        {
            Time.timeScale = 1;
            _isLoadingLevel = true;
            StartCoroutine(RestartLevel());
        }
    }

    public IEnumerator LoadSavedLevel()
    {
        PlayerData playerData = SaveSystem.LoadPlayerData();

        yield return StartCoroutine(LoadLevel(playerData.currentLevelSelect));

        LoadPlayerPositionInLevel(playerData);
    }

    protected override void Awake()
    {
        base.Awake();
        _isPersist = true;
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
        yield return SceneManager.LoadSceneAsync(sceneIndex);

        _isLoadingLevel = false;
    }

    private void LoadPlayerPositionInLevel(PlayerData playerData)
    {
        if (playerData == null) return;
        
        Vector2 playerPosition = new Vector2(playerData.playerPosition[0], playerData.playerPosition[1]);
        GameController.Instance.Player.transform.position = playerPosition;
    }
}
