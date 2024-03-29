using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 4f;
    [SerializeField] private LayerMask _whatIsGround;
    [Range(0, .3f)] [SerializeField] private float _movementSmoothing = 0.05f;

    private Vector3 _velocity = Vector3.zero;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    private Rigidbody2D _rigidbody2D;
    private Collider2D[] _collider2Ds = new Collider2D[10];

    private int _damage = 1;
    private float _speed = 10f;
    private bool _isHoming = false;
    private bool _canHitGround = false;
    private bool _canBeDamaged = false;
    private Sprite _sprite;
    private FireProjectileDirection _fireDirection;
    private float _direction = 1f; //indicates which side the projectile is fired from

    private bool _isDespawning = false;
    private float _despawnTimer = 0f;


    public void SetProjectile(FireProjectile fireProjectile, int damage = 1)
    {
        if (gameObject.activeSelf) return;

        _damage = damage;
        _speed = fireProjectile.speed;
        _isHoming = fireProjectile.isHoming;
        _canHitGround = fireProjectile.canHitGround;
        _canBeDamaged = fireProjectile.canBeDamaged;
        _sprite = fireProjectile.sprite;
        _fireDirection = fireProjectile.fireDirection;
        _direction = fireProjectile.direction;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _rigidbody2D.isKinematic = true;
        _boxCollider2D.isTrigger = true;
    }

    private void OnEnable()
    {
        if (_sprite == null) return;

        _spriteRenderer.sprite = _sprite;
        _boxCollider2D.size = _spriteRenderer.sprite.bounds.size;

        if (!_spriteRenderer.isVisible)
        {
            _isDespawning = true;
        }
    }

    private void OnBecameVisible()
    {
        _isDespawning = false;
        _despawnTimer = 0f;
    }

    private void OnBecameInvisible()
    {
        _isDespawning = true;
    }

    private void Update()
    {
        if(_isDespawning)
        {
            StartDespawnTimer();
        }
    }

    private void StartDespawnTimer()
    {
        if(_despawnTimer < _lifeTime)
        {
            _despawnTimer += Time.deltaTime;
            return;
        }

        _isDespawning = false;
        _despawnTimer = 0f;
        ObjectPoolManager.Instance.DespawnGameObject(gameObject);
    }

    private void FixedUpdate()
    {
        if (_canHitGround)
        {
            CheckGroundCollision();
        }

        ProjectileMove();
    }

    private void CheckGroundCollision()
    {
        int colliders = Physics2D.OverlapBoxNonAlloc(transform.position, _boxCollider2D.size, 0, _collider2Ds, _whatIsGround);
        if (colliders > 0)
        {
            ObjectPoolManager.Instance.DespawnGameObject(gameObject);
        }
    }

    private void ProjectileMove()
    {
        Vector3 targetVelocity = Vector3.zero;

        if(_isHoming)
        {
            targetVelocity = EnemyUtilities.FreeMoveTowardsTarget(ref _speed, transform, GameController.Instance.Player.transform);
        }
        else
        {
            targetVelocity = MoveStraight();
        }

        _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity, _movementSmoothing);
    }

    private Vector3 MoveStraight()
    {
        Vector3 targetVelocity = Vector3.zero;

        _speed = Mathf.Abs(_speed) * _direction;
        float move = (_speed * Time.fixedDeltaTime) * 3f;
        switch (_fireDirection)
        {
            case FireProjectileDirection.FireHorizontally:
                targetVelocity = move * transform.right;
                break;
            case FireProjectileDirection.FireVertically:
                targetVelocity = move * transform.up;
                break;
        }

        return targetVelocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<AttackObjectController>() && _canBeDamaged)
        {
            ObjectPoolManager.Instance.DespawnGameObject(gameObject);
            return;
        }

        if(collision.TryGetComponent<Player>(out Player player))
        {
            if (player.IsInvulnerable) return;

            player.TakeDamage(_damage, EnemyUtilities.GetCollisionDirection(transform, collision));
            ObjectPoolManager.Instance.DespawnGameObject(gameObject);
        }
    }
}
