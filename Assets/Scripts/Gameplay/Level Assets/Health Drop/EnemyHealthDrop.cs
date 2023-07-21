using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class EnemyHealthDrop : MonoBehaviour
{
    [SerializeField] private int _amountToRestore = 1;

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            if (player.CurrentHealth <= 0) return;

            player.RecoverHealth(_amountToRestore);
            ObjectPoolManager.Instance.DespawnGameObject(gameObject);
        }
    }
}
