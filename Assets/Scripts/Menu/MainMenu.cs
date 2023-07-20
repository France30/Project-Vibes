using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Title Screen")]
    [SerializeField] private Button _continueButton;

    [Header("Level Select")]
    [SerializeField] private Button[] _levelSelectButtons;


    private void Awake()
    {
        _continueButton.interactable = SaveSystem.IsSaveFileFound();

        for (int i = 0; i < _levelSelectButtons.Length; i++)
        {
            _levelSelectButtons[i].interactable = false;
        }

        List<int> unlockedLevels = LevelManager.Instance.LevelsUnlocked;
        for (int i = 0; i < unlockedLevels.Count; i++)
        {
            int levelIndex = unlockedLevels[i];
            _levelSelectButtons[levelIndex - 1].interactable = true;
        }
    }

    private void Start()
    {
        AudioManager.Instance.Play("MainMenuBGM");
    }

    private void OnDestroy()
    {
        if (AudioManager.Instance == null) return;

        AudioManager.Instance.Stop("MainMenuBGM");
    }
}
