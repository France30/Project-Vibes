using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int health = 1;

    public bool IsHit { get; set; }

    public void TakeDamage(int value)
    {
        health -= value;
        IsHit = true;
        Debug.Log(gameObject.name + " has been hit");

        if (health <= 0) gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
