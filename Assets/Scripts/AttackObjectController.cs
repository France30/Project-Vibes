using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjectController : MonoBehaviour
{
    [SerializeField] private float maxScale = 2f;
    [SerializeField] private float animationSpeed = 4f;

    private Vector3 originalScale;
    private float damage;

    public float Damage { get { return damage; } set { damage = value; } }
    public bool IsBreakableObjectHit { get; set; }
    public bool IsEnemyHit { get; set; }

    public float AnimationSpeed { get { return animationSpeed; } set { animationSpeed = value; } }

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = originalScale;

        IsBreakableObjectHit = false;
        IsEnemyHit = false;
    }

    private void Update()
    {
        //when the object's local scale exceeds the assigned max scale
        if (transform.localScale.x >= maxScale && transform.localScale.y >= maxScale)
        {
            transform.localScale = originalScale;
            gameObject.SetActive(false);
        }

        //scale object size here, multiplied by animation speed
        float objectScale = animationSpeed * Time.deltaTime;
        Vector3 scaleChange = new Vector3(objectScale, objectScale, 0);
        transform.localScale += scaleChange;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsBreakableObjectHit) 
        {
            IsBreakableObjectHit = true;
        }

        if (!IsEnemyHit)
        {
            IsEnemyHit = true;
        }
    }
}
