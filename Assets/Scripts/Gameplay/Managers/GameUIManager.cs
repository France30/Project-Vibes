using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIManager : Singleton<GameUIManager>
{
    [Header("Game UI")]
    [SerializeField] private GameObject PauseUI;

    [Header("Screen Notification")]
    [SerializeField] private TextMeshProUGUI _notification;
    [SerializeField] private float _fadeSpeed = 1f;

    public TextMeshProUGUI Notification { get { return _notification; } }


    public void FadeInNotificationText()
    {
        if (_notification.alpha < 1f)
        {
            float a = _notification.alpha;
            a += _fadeSpeed * Time.deltaTime;
            SetTextNotifAlpha(a);
        }
    }

    public void FadeOutNotificationText()
    {
        if (_notification.alpha > 0f)
        {
            float a = _notification.alpha;
            a -= _fadeSpeed * Time.deltaTime;
            SetTextNotifAlpha(a);
        }
    }

    public void SetTextNotifAlpha(float a)
    {
        _notification.color = new Color(_notification.color.r, _notification.color.g, _notification.color.b, a);
    }

    private void OnEnable()
    {
        SetTextNotifAlpha(0);
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
