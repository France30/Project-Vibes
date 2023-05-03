using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjectController : MonoBehaviour
{
    [SerializeField] private float maxScale = 2f;
    [SerializeField] private float animationSpeed = 4f;
    [SerializeField] private int damage = 1;

    private Vector3 originalScale;
    private float originalAnimationSpeed;

    private List<EnemyController> enemiesHit = new List<EnemyController>();

    public int Damage { get { return damage; } set { damage = value; } }

    public float AnimationSpeed { get { return animationSpeed; } set { animationSpeed = value; } }
    public int HitboxScaleResetCounter { get; set; } //amount of times the scale should reset before disabling the object

    private void Awake()
    {
        originalScale = transform.localScale;
        originalAnimationSpeed = animationSpeed;
    }

    private void OnEnable()
    {
        transform.localScale = originalScale;
        animationSpeed = originalAnimationSpeed;

        HitboxScaleResetCounter = 1;
    }

    private void Update()
    {
        //when the object's local scale exceeds the assigned max scale
        if (transform.localScale.x >= maxScale && transform.localScale.y >= maxScale)
        {
            ResetEnemyHitFlags();

            transform.localScale = originalScale;
            HitboxScaleResetCounter--;
            if (HitboxScaleResetCounter <= 0) gameObject.SetActive(false);
        }

        //scale object size here, multiplied by animation speed
        float objectScale = animationSpeed * Time.deltaTime;
        Vector3 scaleChange = new Vector3(objectScale, objectScale, 0);
        transform.localScale += scaleChange;
    }

    private void ResetEnemyHitFlags()
    {
        if (enemiesHit.Count <= 0) return;

        foreach (EnemyController enemy in enemiesHit)
        {
            enemy.IsHit = false;
            Debug.Log(enemy.gameObject.name + " hit flag has been reset");
        }

        enemiesHit.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            if (enemy.IsHit) return;

            enemy.TakeDamage(damage);
            enemiesHit.Add(enemy);
        }
    }
}
