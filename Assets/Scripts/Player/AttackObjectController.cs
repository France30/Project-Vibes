using System.Collections.Generic;
using UnityEngine;

public class AttackObjectController : MonoBehaviour
{
    [SerializeField] private float _maxSoundWaveSize = 10f;
    [SerializeField] private float _animationSpeed = 4f;
    [SerializeField] private int _damage = 1;

    private float _maxScale;
    private Vector3 _baseScale;

    private List<int> _damageablesHitID = new List<int>();


    public float MaxScale { get { return _maxScale; } set { _maxScale = value; _maxScale = Mathf.Clamp(_maxScale, 0, _maxSoundWaveSize); } }
    public int Damage { get { return _damage; } set { _damage = value; } }
    public float AnimationSpeed { get { return _animationSpeed; } }
    public float AnimationSpeedMultiplier { get; set; }
    public int HitboxScaleResetCounter { get; set; } //amount of times the scale should reset before disabling the object


    private void Awake()
    {
        _baseScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = _baseScale;

        HitboxScaleResetCounter = 1;
        AnimationSpeedMultiplier = 1f;
    }

    private void Update()
    {
        //clear list on initial scale
        if(transform.localScale == _baseScale)
            ClearDamageablesHitList();

        //when the object's local scale exceeds the assigned max scale
        if (transform.localScale.x >= _maxScale && transform.localScale.y >= _maxScale)
        {
            //Debug.Log(transform.localScale);
            transform.localScale = _baseScale;
            HitboxScaleResetCounter--;
            if (HitboxScaleResetCounter <= 0) gameObject.SetActive(false);
        }

        //scale object size here, multiplied by animation speed
        float animationSpeed = _animationSpeed * AnimationSpeedMultiplier;
        float objectScale = animationSpeed * Time.deltaTime;
        Vector3 scaleChange = new Vector3(objectScale, objectScale, 0);
        transform.localScale += scaleChange;
    }

    private void ClearDamageablesHitList()
    {
        if (_damageablesHitID.Count <= 0) return;

        _damageablesHitID.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            if (_damageablesHitID.Contains(damageable.InstanceID)) return;

            damageable.TakeDamage(_damage);
            _damageablesHitID.Add(damageable.InstanceID);
        }           
    }
}
