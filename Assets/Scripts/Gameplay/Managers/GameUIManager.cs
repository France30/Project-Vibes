using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : Singleton<GameUIManager>
{
    [SerializeField] private GameObject PauseUI;


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
