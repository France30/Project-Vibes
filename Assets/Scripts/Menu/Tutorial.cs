using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject TutorialPrompt;
    [SerializeField] private GameObject TutorialUI;


    public void EnableTutorialUI(bool isEnable)
    {
        TutorialUI.SetActive(isEnable);
        TutorialPrompt.SetActive(false);
        GameController.Instance.DisableGameControls(false);
    }

    private void Awake()
    {
        if (TutorialUI != null)
            TutorialUI.SetActive(false);
    }

    private void Start()
    {
        GameController.Instance.DisableGameControls(true);
        GameController.Instance.OnDisableGameControls += DisableTutorialPrompt;
    }

    private void DisableTutorialPrompt(bool isDisable)
    {
        TutorialPrompt.SetActive(!isDisable);

        if(!isDisable)
        {
            GameController.Instance.OnDisableGameControls -= DisableTutorialPrompt;
            GameController.Instance.DisableGameControls(true);
        }
    }
}
