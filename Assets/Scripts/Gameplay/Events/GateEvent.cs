using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Animator), typeof(SpriteRenderer))]
public class GateEvent : MonoBehaviour
{
    [SerializeField] private Sprite _gateOpenSprite;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Collider2D _gateCollider;

    public void PlayOpenGateAnim()
    {
        _animator.SetBool("Open", true);
    }

    public void OpenGateImmediately()
    {
        _animator.enabled = false;
        _spriteRenderer.sprite = _gateOpenSprite;
        OpenGate();
    }

    public void OpenGate()
    {
        _gateCollider.enabled = false;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _gateCollider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
