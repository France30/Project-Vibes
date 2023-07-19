public class CheckPoint : Interactable
{
    public override void Interact()
    {
        if (AudioManager.Instance.IsPlaying("GameSavedSfx")) return;

        SaveSystem.Save();
        AudioManager.Instance.Play("GameSavedSfx");
        //Debug.Log("Game Saved!");
    }
}
