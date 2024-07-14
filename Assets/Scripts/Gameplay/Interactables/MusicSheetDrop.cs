using UnityEngine;

public class MusicSheetDrop : Interactable
{
	[SerializeField] private MusicSheetSO _musicSheetSO;

	public override void Interact()
	{
		base.Interact();

		PlayerAttack playerAttack = GameController.Instance.Player.GetComponent<PlayerAttack>();
		playerAttack.enabled = false;

		PlayerChords playerChords = playerAttack.PlayerChords;
		playerChords.AddToChordSet(_musicSheetSO);
		playerAttack.enabled = true;

		//GameUIManager.Instance.PlaySongTitleUIPulseAnimation();
		_musicSheetSO.SheetGet();

		gameObject.SetActive(false);
	}

	private void Start()
	{
		gameObject.SetActive(_musicSheetSO.IsSheetInScene);
	}
}
