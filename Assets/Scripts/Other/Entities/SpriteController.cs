using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteController : MonoBehaviour
{
    [SerializeField] private SpriteFlash _spriteFlash;

    private SpriteRenderer _spriteRenderer;
    private Material _originalMaterial;
    private int _currentFlashCount = 0;
    private bool _isFlashing = false;

    public delegate void FlashEvent(bool value);
    public event FlashEvent OnFlashEvent;

    public bool IsFlashing { get { return _isFlashing; } }

    public IEnumerator Flash()
    {
        WaitForSeconds waitForFlashSpeed = new WaitForSeconds(_spriteFlash.FlashSpeed);

        if (_currentFlashCount <= 0)
            OnFlashEvent?.Invoke(true);

        _spriteRenderer.material = _spriteFlash.FlashMaterial;

        yield return waitForFlashSpeed;

        _spriteRenderer.material = _originalMaterial;
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
    }
}
