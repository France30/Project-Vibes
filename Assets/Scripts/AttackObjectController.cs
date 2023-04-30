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

    public bool IsBreakableObjectHit { get; set; }
    public bool IsEnemyHit { get; set; }
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

        IsBreakableObjectHit = false;
        IsEnemyHit = false;
        HitboxScaleResetCounter = 1;
    }

    private void Update()
    {
        //when the object's local scale exceeds the assigned max scale
        if (transform.localScale.x >= maxScale && transform.localScale.y >= maxScale)
        {
            transform.localScale = originalScale;
            HitboxScaleResetCounter--;
            if (HitboxScaleResetCounter <= 0) gameObject.SetActive(false);
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
