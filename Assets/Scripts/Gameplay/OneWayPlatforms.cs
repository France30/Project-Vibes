using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(PlatformEffector2D))]
public class OneWayPlatforms : MonoBehaviour
{
    private PlatformEffector2D _platformEffector2D;
    private bool _isPlayerOnPlatform = false;


    private void Awake()
    {
        _platformEffector2D = GetComponent<PlatformEffector2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && _isPlayerOnPlatform)
            _platformEffector2D.rotationalOffset = 180;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<Player>()) return;

        _isPlayerOnPlatform = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<Player>()) return;

        if (_isPlayerOnPlatform && _platformEffector2D.rotationalOffset == 180)
            _platformEffector2D.rotationalOffset = 0;

        _isPlayerOnPlatform = false;

        Debug.Log("Player not on one-way platform");
    }
}
