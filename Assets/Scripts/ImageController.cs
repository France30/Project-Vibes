using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    [SerializeField] private float FadeSpeed = 1f;

    private Image gameObjectImage;

    public float ImageAlpha { get; set; }

    private void Awake()
    {
        gameObjectImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObjectImage.color = new Color(gameObjectImage.color.r, gameObjectImage.color.g, gameObjectImage.color.b, ImageAlpha);

        if (ImageAlpha > 0)
            ImageAlpha -= FadeSpeed * Time.deltaTime;
    }
}
