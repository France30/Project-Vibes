using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteController : MonoBehaviour
{
    [SerializeField] private SpriteFlash _spriteFlash;

    private SpriteRenderer _spriteRenderer;
    private Material _originalMaterial;
    private int _currentFlashCount = 0;
    private bool _isFlashing = false;
    private Vector2 _spriteSize;

    public delegate void FlashEvent(bool isFlashing);
    public event FlashEvent OnFlashEvent;

    public SpriteRenderer SpriteRenderer { 
        get 
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();

            return _spriteRenderer;
        } 
    }
    public Vector2 SpriteSize { 
        get 
        {
            if (_spriteSize == null || _spriteSize == Vector2.zero)
                _spriteSize = SpriteRenderer.sprite.bounds.size;

            return _spriteSize;
        } 
    }
    public bool IsFlashing { get { return _isFlashing; } }


    public IEnumerator Flash()
    {
        WaitForSeconds waitForFlashSpeed = new WaitForSeconds(_spriteFlash.FlashSpeed);

        if (_currentFlashCount <= 0)
            OnFlashEvent?.Invoke(true);

        SpriteRenderer.material = _spriteFlash.FlashMaterial;

        yield return waitForFlashSpeed;

        SpriteRenderer.material = _originalMaterial;
        _currentFlashCount++;

        yield return waitForFlashSpeed;

        CheckFlashCount();
    }

    private void CheckFlashCount()
    {
        if (_currentFlashCount < _spriteFlash.FlashCount)
        {
            StartCoroutine(Flash());
        }
        else if (_currentFlashCount >= _spriteFlash.FlashCount)
        {
            _currentFlashCount = 0;
            OnFlashEvent?.Invoke(false);
        }
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalMaterial = _spriteRenderer.material;
        _spriteSize = _spriteRenderer.sprite.bounds.size;
    }

    private void OnEnable()
    {
        OnFlashEvent += SetIsFlashing;
    }

    private void OnDisable()
    {
        OnFlashEvent -= SetIsFlashing;
    }

    private void SetIsFlashing(bool isFlashing)
    {
        _isFlashing = isFlashing;
    }
}
