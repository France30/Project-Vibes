using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _maxHealth;

    [SerializeField] private float _hurtTime = 1f;
    [SerializeField] private Vector2 _knockBackForce;

    private Rigidbody2D _rigidbody2D;
    private Health _health;


    public void TakeDamage(int value, int knockBackDirection)
    {
        if (_isInvulnerable) return;

        _health.CurrentHealth -= value;

        //Apply KnockBack
        _rigidbody2D.velocity = Vector2.zero;
        float horizontalForce = _knockBackForce.x * knockBackDirection;
        _rigidbody2D.AddForce(new Vector2(horizontalForce, _knockBackForce.y), ForceMode2D.Impulse);

    }

    private void Awake()
    {
        _health = new Health(_maxHealth);
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
}
