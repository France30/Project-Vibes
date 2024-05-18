using UnityEngine;
using TMPro;


[RequireComponent(typeof(BoxCollider2D))]
public abstract class Interactable : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _interactableText;
	[SerializeField] private EnemyBase[] _surroundingEnemies;

	public TextMeshProUGUI InteractableText { get { return _interactableText; } }

	//The default behaviour for interactables should save player position & kill surrounding perma death enemies
	public virtual void Interact()
    {
		SaveSystem.SavePlayerData();
		for (int i = 0; i < _surroundingEnemies.Length; i++)
		{
			_surroundingEnemies[i].TakeDamage(_surroundingEnemies[i].MaxHealth);
		}
	}

	protected virtual void Awake()
	{
		_interactableText.enabled = false;
		GetComponent<BoxCollider2D>().isTrigger = true;
	}

	private void OnDisable()
	{
		_interactableText.enabled = false;
	}
}
