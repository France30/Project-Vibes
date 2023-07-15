using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIManager : Singleton<GameUIManager>
{
    [SerializeField] private GameObject PauseUI;

    [SerializeField] private TextMeshProUGUI _textNotif;
    [SerializeField] private float _fadeSpeed = 1f;

    public TextMeshProUGUI TextNotif { get { return _textNotif; } }

    private void OnEnable()
    {
        PauseUI.SetActive(false);
        GameController.Instance.OnPauseEvent += EnablePauseUI;
    }

    private void OnDisable()
    {
        if (GameController.Instance == null) return;

        GameController.Instance.OnPauseEvent -= EnablePauseUI;
    }

    private void EnablePauseUI(bool isEnable)
    {
        PauseUI.SetActive(isEnable);
    }
}
