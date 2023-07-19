using UnityEngine;

public class MusicSheet : Interactable
{
    [SerializeField] private MusicSheetSO _musicSheetSO;


    public override void Interact()
    {
        SaveSystem.Save(); //Music Sheet Pick-ups also double as checkpoints

        PlayerAttack playerAttack = GameController.Instance.Player.GetComponent<PlayerAttack>();
        playerAttack.enabled = false;

        PlayerChords playerChords = playerAttack.PlayerChords;
        playerChords.AddToChordSet(_musicSheetSO);
        playerAttack.enabled = true;

        _musicSheetSO.SheetGet();
        gameObject.SetActive(false);
    }

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(_musicSheetSO.IsSheetInScene);
    }
}
