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

    private bool _isGateOpen = false;

    public void PlayOpenGateAnim()
    {
        _isGateOpen = true;
        _animator.SetBool("Open", true);
    }

    public void OpenGateImmediately()
    {
        if (_isGateOpen) return;

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
