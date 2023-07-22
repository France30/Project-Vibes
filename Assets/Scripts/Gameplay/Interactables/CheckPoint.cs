public class CheckPoint : Interactable
{
    public override void Interact()
    {
        if (AudioManager.Instance.IsPlaying("GameSavedSfx")) return;

        SaveSystem.SavePlayerData();
        SaveSystem.SaveCheckpointData(_id);
        AudioManager.Instance.Play("GameSavedSfx");
    }
}
