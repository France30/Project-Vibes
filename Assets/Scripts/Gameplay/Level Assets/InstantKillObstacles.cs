using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class InstantKillObstacles : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(player.MaxHealth);
        }

        if(collision.gameObject.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            enemy.TakeDamage(enemy.MaxHealth);
        }
    }
}
