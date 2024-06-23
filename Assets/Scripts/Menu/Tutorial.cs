using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _tutorialPrompt;
    [SerializeField] private GameObject _tutorialUI;
    [SerializeField] private TutorialData _tutorialData;


    public void EnableTutorialUI(bool isEnable)
    {
        _tutorialData.EnableTutorial(isEnable);

        _tutorialUI.SetActive(isEnable);
        _tutorialPrompt.SetActive(false);
        GameController.Instance.DisableGameControls(false);

        if(isEnable)
            GameController.Instance.OnDisableGameControls += DisableTutorialUI;
    }

    private void Start()
    {
        GameController.Instance.OnDisableGameControls += DisableTutorialPrompt;

        if (!SaveSystem.IsSaveFileFound())
            _tutorialData.Reset();

        if (_tutorialData.IsTutorialEnabled)
        {
            _tutorialPrompt.SetActive(false);
            _tutorialUI.SetActive(true);

            GameController.Instance.OnDisableGameControls += DisableTutorialUI;
            GameController.Instance.OnDisableGameControls -= DisableTutorialPrompt;
        }
        else
        {
            if (_tutorialUI != null)
                _tutorialUI.SetActive(false);

            if(SaveSystem.IsSaveFileFound() || LevelManager.Instance.CurrentLevel > 1)
            {
                _tutorialPrompt.SetActive(false);
                GameController.Instance.OnDisableGameControls -= DisableTutorialPrompt;
            }
            else
                GameController.Instance.DisableGameControls(true);
        }
    }

    private void DisableTutorialPrompt(bool isDisable)
    {
        _tutorialPrompt.SetActive(!isDisable);

        if(!isDisable)
        {
            GameController.Instance.OnDisableGameControls -= DisableTutorialPrompt;
            GameController.Instance.DisableGameControls(true);
        }
    }

    private void DisableTutorialUI(bool isDisable)
    {
        _tutorialUI.SetActive(!isDisable);
    }
}
