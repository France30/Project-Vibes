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

    public delegate void PlayerDeath(bool isPlayerDead);
    public event PlayerDeath OnPlayerDeath;

    public int MaxHealth { get { return _maxHealth; } }


    public void TakeDamage(int value, int knockBackDirection = 0)
    {
        if (_spriteController.IsFlashing || _health.CurrentHealth <= 0) return;

        _health.CurrentHealth -= value;

        if (_health.CurrentHealth <= 0)
        {
            OnPlayerDeath?.Invoke(true);
            return;
        }

        ApplyKnockBack(knockBackDirection);
        StartCoroutine(HurtDuration());
        StartCoroutine(_spriteController.Flash()); //to move, keep here for prototype/alpha purposes
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
        bool isHurt = true;
        _animator.SetBool("Hurt", isHurt);
        DisablePlayerActions(isHurt);

        yield return new WaitForSeconds(_hurtTime);

        _animator.SetBool("Hurt", !isHurt);
        DisablePlayerActions(!isHurt);
    }

    private void DisablePlayerActions(bool isEnable)
    {
        for (int i = 0; i < _playerActions.Length; i++)
            _playerActions[i].enabled = !isEnable;
    }
}
