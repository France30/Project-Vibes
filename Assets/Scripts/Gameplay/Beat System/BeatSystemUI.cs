using UnityEngine;
using UnityEngine.UI;

public class BeatSystemUI : MonoBehaviour
{
	[SerializeField] private float _fadeSpeed = 1f;

	private Image _image;

	public float ImageAlpha { get; set; }

	private void Awake()
	{
		_image = GetComponent<Image>();
	}

    private void Start()
    {
#if UNITY_EDITOR
		GameController.Instance.OnDisableGameHUD += DisableUI;
#endif
	}

    private void Update()
	{
		_image.color = new Color(_image.color.r, _image.color.g, _image.color.b, ImageAlpha);

		if (ImageAlpha > 0)
			ImageAlpha -= _fadeSpeed * Time.deltaTime;
	}
#if UNITY_EDITOR
	private void DisableUI(bool isDisable)
    {
		_image.enabled = !isDisable;
    }
#endif
}
