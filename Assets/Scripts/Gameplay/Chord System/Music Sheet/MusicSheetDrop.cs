using UnityEngine;

public class MusicSheetDrop : Interactable
{
	[SerializeField] private MusicSheetSO _musicSheetSO;


	public override void Interact()
	{
		SaveSystem.SavePlayerData(); //Music Sheet Pick-ups also double as checkpoints

		PlayerAttack playerAttack = GameController.Instance.Player.GetComponent<PlayerAttack>();
		playerAttack.enabled = false;

		PlayerChords playerChords = playerAttack.PlayerChords;
		playerChords.AddToChordSet(_musicSheetSO);
		playerAttack.enabled = true;

		GameUIManager.Instance.PlaySongTitleUIPulseAnimation();
		_musicSheetSO.SheetGet();
		gameObject.SetActive(false);
	}

	protected override void Awake()
	{
		base.Awake();
		gameObject.SetActive(_musicSheetSO.IsSheetInScene);
	}
}
