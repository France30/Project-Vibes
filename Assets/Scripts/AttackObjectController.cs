using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackObjectController : MonoBehaviour
{
    //might replace variables with scriptable objects
    //using this format for prototype
    [SerializeField] private float animationSpeed = 4f;

    public float AnimationSpeed { get; set; }

    private void Awake()
    {
    }

    private void Update()
    {
        //scale object size here, multiplied by animation speed
        float objectScale = animationSpeed * Time.deltaTime;
        Vector3 scaleChange = new Vector3(objectScale, objectScale, 0);
        transform.localScale += scaleChange;
    }
}
