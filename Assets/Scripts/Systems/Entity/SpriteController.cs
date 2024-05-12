using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteController : MonoBehaviour
{
	[SerializeField] private SpriteFlash _spriteFlash;
	[SerializeField] private SpriteRenderer[] _customSprite;
	
	private SpriteRenderer _spriteRenderer;
	private Material _originalMaterial;
	private int _currentFlashCount = 0;
	private bool _isFlashing = false;
	private Vector2 _spriteSize;

	public delegate void FlashEvent(bool isFlashing);
	public event FlashEvent OnFlashEvent;

	public Vector2 SpriteSize { 
		get 
		{
			if (_spriteRenderer == null)
				_spriteSize = Vector2.zero;
			else if (_spriteSize == null || _spriteSize == Vector2.zero)
				_spriteSize = _spriteRenderer.sprite.bounds.size;

			return _spriteSize;
		} 
	}
	public bool IsFlashing { get { return _isFlashing; } }


	public IEnumerator Flash()
	{
		WaitForSeconds waitForFlashSpeed = new WaitForSeconds(_spriteFlash.FlashSpeed);

		if (_currentFlashCount <= 0)
			OnFlashEvent?.Invoke(true);

		if (_spriteRenderer != null && _spriteRenderer.enabled)
			_spriteRenderer.material = _spriteFlash.FlashMaterial;
		else if (_customSprite.Length > 0)
			FlashCustomSprite(_spriteFlash.FlashMaterial);

		yield return waitForFlashSpeed;

		if (_spriteRenderer != null && _spriteRenderer.enabled)
			_spriteRenderer.material = _originalMaterial;
		else if (_customSprite.Length > 0)
			FlashCustomSprite(_spriteRenderer.material);

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

	private void FlashCustomSprite(Material material)
    {
		for (int i = 0; i < _customSprite.Length; i++)
		{
			_customSprite[i].material = material;
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
