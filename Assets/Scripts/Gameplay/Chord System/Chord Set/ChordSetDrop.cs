using UnityEngine;

public class ChordSetDrop : Interactable
{
    [SerializeField] private ChordSet _chordSet;

    public override void Interact()
    {
        PlayerAttack playerAttack = GameController.Instance.Player.GetComponent<PlayerAttack>();
        playerAttack.enabled = false;

        PlayerChords playerChords = playerAttack.PlayerChords;
        playerChords.AddChordSet(_chordSet);
        playerAttack.enabled = true;

        GameUIManager.Instance.PlaySongTitleUIPulseAnimation();
        _chordSet.ChordSetSO.DropGet();
        gameObject.SetActive(false);
    }

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(_chordSet.ChordSetSO.IsDrop);
    }
}
