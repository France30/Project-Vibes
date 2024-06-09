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

    public bool IsGateOpen { get; private set; }
    public void PlayOpenGateAnim()
    {

    }

    public void OpenGateImmediately()
    {
        _spriteRenderer.sprite = _gateOpenSprite;
        OpenGate();
    }

    public void OpenGate()
    {
        IsGateOpen = true;
        _gateCollider.enabled = false;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _gateCollider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        IsGateOpen = false;
    }
}
