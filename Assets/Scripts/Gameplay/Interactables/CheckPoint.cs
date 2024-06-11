using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CheckPoint : Interactable
{
	[SerializeField] private string _id;

	private Animator _animator;

	public override void Interact()
	{
		if (AudioManager.Instance.IsPlaying("GameSavedSfx")) return;

		base.Interact();

		SaveSystem.SaveCheckpointData(_id);

		if (_gateEvent != null)
			SaveSystem.SaveGateEventData(_id);

		GameController.Instance.Player.SavedCheckPoint = _id;
		_animator.SetBool("On", true);
		AudioManager.Instance.Play("GameSavedSfx");
	}

	protected override void Awake()
	{
		base.Awake();
		_animator = GetComponent<Animator>();
	}

    private void Start()
    {
		if(_gateEvent != null && SaveSystem.IsGateEventSaveFound(_id))
			_gateEvent.OpenGateImmediately();
    }

    private void OnEnable()
	{
		bool isThisPlayerSpawnPoint = (_id == GameController.Instance.Player.SavedCheckPoint);
		_animator.SetBool("On", isThisPlayerSpawnPoint);
	}

	private void OnBecameVisible()
	{
		bool isThisPlayerSpawnPoint = (_id == GameController.Instance.Player.SavedCheckPoint);
		_animator.SetBool("On", isThisPlayerSpawnPoint);
	}
}
