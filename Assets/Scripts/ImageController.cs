using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 1f;

    private Image image;

    public float ImageAlpha { get; set; }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, ImageAlpha);

        if (ImageAlpha > 0)
            ImageAlpha -= fadeSpeed * Time.deltaTime;
    }
}
