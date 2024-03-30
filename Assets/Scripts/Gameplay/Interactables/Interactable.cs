using UnityEngine;
using TMPro;


[RequireComponent(typeof(BoxCollider2D))]
public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _interactableText;

    public TextMeshProUGUI InteractableText { get { return _interactableText; } }


    public abstract void Interact();

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
