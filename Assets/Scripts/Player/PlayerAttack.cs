using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject attackObject;

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetButton("Fire1") && !attackObject.activeSelf)
        {
            attackObject.SetActive(true);
        }
    }
}
