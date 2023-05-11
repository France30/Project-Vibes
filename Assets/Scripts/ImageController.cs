using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    [SerializeField] private float _fadeSpeed = 1f;

    private Image _image;

    public float ImageAlpha { get; set; }

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, ImageAlpha);

        if (ImageAlpha > 0)
            ImageAlpha -= _fadeSpeed * Time.deltaTime;
    }
}
