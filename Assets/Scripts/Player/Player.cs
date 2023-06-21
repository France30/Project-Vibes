using System.Collections;
using UnityEngine;


[RequireComponent(typeof(SpriteController))]
public class Player : MonoBehaviour
{
    [SerializeField] private int _maxHealth;

    [SerializeField] private float _hurtTime = 1f;
    [SerializeField] private Vector2 _knockBackForce;

    [Space]
    [SerializeField] private MonoBehaviour[] _playerActions;

    private Animator _animator;
    private SpriteController _spriteController;
    private Rigidbody2D _rigidbody2D;
    private Health _health;


    public void TakeDamage(int value, int knockBackDirection)
    {
        if (_spriteController.IsFlashing) return;

        _health.CurrentHealth -= value;

        //Apply KnockBack
        _rigidbody2D.velocity = Vector2.zero;
        float horizontalForce = _knockBackForce.x * knockBackDirection;
        _rigidbody2D.AddForce(new Vector2(horizontalForce, _knockBackForce.y), ForceMode2D.Impulse);

        StartCoroutine(HurtDuration());
        StartCoroutine(_spriteController.Flash());
    }

    private void Awake()
    {
        _health = new Health(_maxHealth);
        _animator = GetComponent<Animator>();
        _spriteController = GetComponent<SpriteController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

    }

    private IEnumerator HurtDuration()
    {
        _animator.SetBool("Hurt", true);
        EnablePlayerActions(false);

        yield return new WaitForSeconds(_hurtTime);

        _animator.SetBool("Hurt", false);
        EnablePlayerActions(true);
    }

    private void EnablePlayerActions(bool isEnable)
    {
        for (int i = 0; i < _playerActions.Length; i++)
            _playerActions[i].enabled = isEnable;
    }
}
