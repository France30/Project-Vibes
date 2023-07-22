using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CheckPoint : Interactable
{
    [SerializeField] private string _id;

    private Animator _animator;

    public override void Interact()
    {
        if (AudioManager.Instance.IsPlaying("GameSavedSfx")) return;

        SaveSystem.SavePlayerData();
        SaveSystem.SaveCheckpointData(_id);
        _animator.SetBool("On", true);
        AudioManager.Instance.Play("GameSavedSfx");
    }

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        bool isThisPlayerSpawnPoint = (_id == SaveSystem.LoadCheckpointData());
        _animator.SetBool("On", isThisPlayerSpawnPoint);
    }

    private void OnBecameVisible()
    {
        bool isThisPlayerSpawnPoint = (_id == SaveSystem.LoadCheckpointData());
        _animator.SetBool("On", isThisPlayerSpawnPoint);
    }
}
