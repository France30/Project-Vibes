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

	public delegate void PlayerEvent(bool isPlayerEvent);
	public event PlayerEvent OnPlayerDeath;
	public event PlayerEvent OnPlayerHurt;

	public bool IsInvulnerable { get { return _isHurt || _spriteController.IsFlashing; } }
	public int MaxHealth { get { return (int)_health.MaxHealth; } }
	public int CurrentHealth { get { return (int)_health.CurrentHealth; } }
	public string SavedCheckPoint { get; set; }


	public void TakeDamage(int value, int knockBackDirection = 0)
	{
		if (_health.CurrentHealth <= 0) return;

		_isHurt = true;
		SetHurtAnimation(_isHurt);

		_health.CurrentHealth -= value;
		_animator.SetFloat("Health", _health.CurrentHealth);
		if (_health.CurrentHealth <= 0)
		{
			StopAllCoroutines();
			OnPlayerDeath?.Invoke(true);
			return;
		}

		StartCoroutine(HurtDuration());
		ApplyKnockBack(knockBackDirection);
	}

	public void RecoverHealth(int value)
	{
		if (_health.CurrentHealth <= 0) return;

		_health.CurrentHealth += value;
	}

	private void Awake()
	{
		DisablePlayerActions(true);

		_animator = GetComponent<Animator>();
		_spriteController = GetComponent<SpriteController>();
		_rigidbody2D = GetComponent<Rigidbody2D>();

		_health = new Health(_maxHealth, _healthBar);
		_animator.SetFloat("Health", _health.CurrentHealth);
		SavedCheckPoint = SaveSystem.LoadCheckpointData();
	}

    private void OnEnable()
	{
		OnPlayerDeath += DisablePlayerActions;
		GameController.Instance.OnFreezeEffect += SetHurtAnimation;
		GameController.Instance.OnPauseEvent += DisablePlayerActions;
		GameController.Instance.OnPrologueEnd += DisablePlayerActions;
		GameController.Instance.OnDisableGameControls += DisablePlayerActions;

		if(GameController.Instance.Boss != null)
		{
			GameController.Instance.Boss.EnemyDeathSequence.OnAnimationStart += StopAllCoroutines;
			GameController.Instance.Boss.EnemyDeathSequence.OnAnimationEnd += PlayVictoryAnimation;
		}
	}

	private void OnDisable()
	{
		OnPlayerDeath -= DisablePlayerActions;

		if (GameController.Instance == null) return;

		GameController.Instance.OnFreezeEffect -= SetHurtAnimation;
		GameController.Instance.OnPauseEvent -= DisablePlayerActions;
		GameController.Instance.OnPrologueEnd -= DisablePlayerActions;
		GameController.Instance.OnDisableGameControls -= DisablePlayerActions;

		if (GameController.Instance.Boss != null)
		{
			GameController.Instance.Boss.EnemyDeathSequence.OnAnimationStart -= StopAllCoroutines;
			GameController.Instance.Boss.EnemyDeathSequence.OnAnimationEnd -= PlayVictoryAnimation;
		}
	}

	private void ApplyKnockBack(int knockBackDirection = 0)
	{
		_rigidbody2D.velocity = Vector2.zero;
		float horizontalForce = _knockBackForce.x * knockBackDirection;
		_rigidbody2D.AddForce(new Vector2(horizontalForce, _knockBackForce.y), ForceMode2D.Impulse);
	}

	private IEnumerator HurtDuration()
	{
		DisablePlayerActions(_isHurt);
		OnPlayerHurt?.Invoke(_isHurt);

		yield return new WaitForSeconds(_hurtTime);

		SetHurtAnimation(!_isHurt);
		DisablePlayerActions(!_isHurt);
		OnPlayerHurt?.Invoke(!_isHurt);

		StartCoroutine(_spriteController.Flash()); //begin Invincibility Frames
		_isHurt = false;
	}

	private void SetHurtAnimation(bool isHurt)
	{
		_animator.SetBool("Hurt", isHurt);
	}

	private void DisablePlayerActions(bool isEnable)
	{
		for (int i = 0; i < _playerActions.Length; i++)
			_playerActions[i].enabled = !isEnable;
	}

	private void PlayVictoryAnimation()
	{
		_animator.SetBool("Victory", true);
	}
}
