public class CheckPoint : Interactable
{
    public override void Interact()
    {
        if (AudioManager.Instance.IsPlaying("GameSavedSfx")) return;

        SaveSystem.SavePlayerData();
        AudioManager.Instance.Play("GameSavedSfx");
    }
}
