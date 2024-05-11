using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : Singleton<GameUIManager>
{
	[Header("Game UI")]
	[SerializeField] private GameObject PauseUI;

	[Header("Song Title UI")]
	[SerializeField] private TextMeshProUGUI _songTitleUI;
	[SerializeField] private Animator _songTitleUIAnimator;

	[Header("Screen Notification")]
	[SerializeField] private TextMeshProUGUI _notification;
	[SerializeField] private float _fadeSpeed = 1f;

	[Header("Temporary Prologue Win UI")]
	[SerializeField] private Image _winUI;
	[SerializeField] private float _winFadeSpeed = 1f;

	[Header("Chord Extender UI")]
	[SerializeField] private GameObject AddedSheetUI;
	[SerializeField] private GameObject MissingSheetUI;

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

	public void SetSongTitleUI(string songTitle)
	{
		_songTitleUI.text = "| " + songTitle;
	}

	public void PlaySongTitleUIPulseAnimation()
	{
		_songTitleUIAnimator.SetTrigger("Pulse");
	}

    public void RefreshChordExtenderUI()
    {
		var addedSheetsUI = AddedSheetUI.transform.childCount;
        for (int i = 0; i < addedSheetsUI; i++)
        {
			AddedSheetUI.transform.GetChild(i).gameObject.SetActive(false);
		}

		var missingSheetsUI = MissingSheetUI.transform.childCount;
		for (int i = 0; i < missingSheetsUI; i++)
		{
			MissingSheetUI.transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	public void UpdateChordExtenderUI(ChordSet chordSet)
    {
		var musicSheets = chordSet.MusicSheets;
		for(int i = 0; i < musicSheets.Length; i++)
        {
			if (musicSheets[i].isAddedToChordSet)
			{
				MissingSheetUI.transform.GetChild(i).gameObject.SetActive(false);
				AddedSheetUI.transform.GetChild(i).gameObject.SetActive(true);
			}
			else
				MissingSheetUI.transform.GetChild(i).gameObject.SetActive(true);
		}
    }

    private void OnEnable()
	{
		SetTextNotifAlpha(0);
		PauseUI.SetActive(false);
		GameController.Instance.OnPauseEvent += EnablePauseUI;

		_winUI.color = new Color(_notification.color.r, _notification.color.g, _notification.color.b, 0);
		if (GameController.Instance.Boss != null)
		{
			GameController.Instance.Boss.EnemyDeathSequence.OnAnimationEnd += BeginWinUIFadeIn;
		}
	}

	private void OnDisable()
	{
		if (GameController.Instance == null) return;

		GameController.Instance.OnPauseEvent -= EnablePauseUI;
		if (GameController.Instance.Boss != null)
		{
			GameController.Instance.Boss.EnemyDeathSequence.OnAnimationEnd -= BeginWinUIFadeIn;
		}
	}

	private void EnablePauseUI(bool isEnable)
	{
		PauseUI.SetActive(isEnable);
	}

	private void BeginWinUIFadeIn()
	{
		StartCoroutine(FadeInWinUI());
	}

	private IEnumerator FadeInWinUI()
	{
		float a = 0;
		while (a < 1f)
		{
			_winUI.color = new Color(_notification.color.r, _notification.color.g, _notification.color.b, a);
			a += _winFadeSpeed * Time.deltaTime;
			yield return null;
		}

		LevelManager.Instance.LoadLevelSelect(0); //return to Main Menu
	}
}
