using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _continueButton;


    private void Awake()
    {
        _continueButton.interactable = SaveSystem.IsSaveFileFound();
    }

    private void Start()
    {
        AudioManager.Instance.Play("MainMenuBGM");
    }
}
