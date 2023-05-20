using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjectController : MonoBehaviour
{
    [SerializeField] private float _maxScale = 2f;
    [SerializeField] private float _animationSpeed = 4f;
    [SerializeField] private int _damage = 1;

    private Vector3 _baseScale;
    private float _baseAnimationSpeed;

    private List<EnemyBase> _enemiesHit = new List<EnemyBase>();

    public int Damage { get { return _damage; } set { _damage = value; } }

    public float AnimationSpeed { get { return _animationSpeed; } set { _animationSpeed = value; } }
    public int HitboxScaleResetCounter { get; set; } //amount of times the scale should reset before disabling the object

    private void Awake()
    {
        _baseScale = transform.localScale;
        _baseAnimationSpeed = _animationSpeed;
    }

    private void OnEnable()
    {
        transform.localScale = _baseScale;
        _animationSpeed = _baseAnimationSpeed;

        HitboxScaleResetCounter = 1;
    }

    private void Update()
    {
        //when the object's local scale exceeds the assigned max scale
        if (transform.localScale.x >= _maxScale && transform.localScale.y >= _maxScale)
        {
            ResetEnemyHitFlags();

            transform.localScale = _baseScale;
            HitboxScaleResetCounter--;
            if (HitboxScaleResetCounter <= 0) gameObject.SetActive(false);
        }

        //scale object size here, multiplied by animation speed
        float objectScale = _animationSpeed * Time.deltaTime;
        Vector3 scaleChange = new Vector3(objectScale, objectScale, 0);
        transform.localScale += scaleChange;
    }

    private void ResetEnemyHitFlags()
    {
        if (_enemiesHit.Count <= 0) return;

        foreach (EnemyBase enemy in _enemiesHit)
        {
            enemy.IsHit = false;
            Debug.Log(enemy.gameObject.name + " hit flag has been reset");
        }

        _enemiesHit.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            if (enemy.IsHit) return;

            enemy.TakeDamage(_damage);
            _enemiesHit.Add(enemy);
        }
    }
}
