using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
	[SerializeField] private float _interactRadius = 2f;
	[SerializeField] private LayerMask _whatIsInteractable;

	private Interactable _interactable;
	private CircleCollider2D[] _interactCollider = new CircleCollider2D[1];


	private void Update()
	{
		if (_interactable == null) return;

		if (Input.GetKeyDown(KeyCode.F))
		{
			_interactable.Interact();
			_interactable = null;
		}
	}

	private void FixedUpdate()
	{
		int interactables = Physics2D.OverlapCircleNonAlloc(transform.position, _interactRadius, _interactCollider, _whatIsInteractable);
		if (interactables > 0 && _interactable == null)
		{
			_interactable = _interactCollider[0].GetComponent<Interactable>();
			_interactable.InteractableText.enabled = true;
		}
		else if (interactables <= 0 && _interactable != null)
		{
			_interactable.InteractableText.enabled = false;
			_interactable = null;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, _interactRadius);
	}
}
