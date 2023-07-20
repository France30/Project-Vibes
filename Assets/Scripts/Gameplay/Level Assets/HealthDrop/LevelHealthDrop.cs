using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelHealthDrop : MonoBehaviour
{
    [SerializeField] private LevelHealthDropSO _levelHealthDropSO;

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
        gameObject.SetActive(_levelHealthDropSO.IsDropInScene);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            if (player.CurrentHealth <= 0) return;

            player.RecoverHealth(_levelHealthDropSO.AmountToRestore);
            _levelHealthDropSO.DropGet();
            gameObject.SetActive(false);
        }
    }
}
