using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(SpriteController))]
public class Player : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private int _maxHealth;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _healthBarOverlay;

    [Header("Player Hit")]
    [SerializeField] private float _hurtTime = 1f;
    [SerializeField] private Vector2 _knockBackForce;

    [Space]
    [SerializeField] private MonoBehaviour[] _playerActions;

    private Animator _animator;
    private SpriteController _spriteController;
    private Rigidbody2D _rigidbody2D;
    private Health _health;

    private bool _isHurt = false;
    private bool _isDead = false;

    public delegate void PlayerDeath(bool isPlayerDead);
    public event PlayerDeath OnPlayerDeath;


    public void TakeDamage(int value, int knockBackDirection = 0)
    {
        if (_isHurt || _spriteController.IsFlashing) return;

        _health.CurrentHealth -= value;
        _isHurt = true;

        if (_health.CurrentHealth <= 0)
        {
            OnPlayerDeath?.Invoke(true);
            return;
        }

        StartCoroutine(HurtDuration());
        ApplyKnockBack(knockBackDirection);
        StartCoroutine(_spriteController.Flash()); //for placeholder purposes only
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteController = GetComponent<SpriteController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _health = new Health(_maxHealth, _healthBar);
    }

    private void OnEnable()
    {
        OnPlayerDeath += DisablePlayerActions;
        GameController.Instance.OnPauseEvent += DisablePlayerActions;
    }

    private void OnDisable()
    {
        OnPlayerDeath -= DisablePlayerActions;

        if (GameController.Instance == null) return;

        GameController.Instance.OnPauseEvent -= DisablePlayerActions;
    }

    private void ApplyKnockBack(int knockBackDirection = 0)
    {
        _rigidbody2D.velocity = Vector2.zero;
        float horizontalForce = _knockBackForce.x * knockBackDirection;
        _rigidbody2D.AddForce(new Vector2(horizontalForce, _knockBackForce.y), ForceMode2D.Impulse);
    }

    private IEnumerator HurtDuration()
    {
        _animator.SetBool("Hurt", _isHurt);
        DisablePlayerActions(_isHurt);

        yield return new WaitForSeconds(_hurtTime);

        _animator.SetBool("Hurt", !_isHurt);
        DisablePlayerActions(!_isHurt);

        //StartCoroutine(_spriteController.Flash());
        _isHurt = false;
    }

    private void DisablePlayerActions(bool isEnable)
    {
        for (int i = 0; i < _playerActions.Length; i++)
            _playerActions[i].enabled = !isEnable;
    }
}
