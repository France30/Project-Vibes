using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private float _maxHealth;

    [Header("Player Movement")]
    [SerializeField] private float _moveSpeed = 40f;

    [Header("Player Attack")]
    [SerializeField] private GameObject _attackObject;
    [SerializeField] private Chord _chord;
    [SerializeField] private GameObject _chordAudioSource;
    [SerializeField] private float _penaltyCooldown = 3.0f;

    private Health _health;
    private PlayerMovement _movement;
    private PlayerAttack _attack;


    private void Awake()
    {
        _health = new Health(_maxHealth);

        InitializePlayerMovement();
        InitializePlayerAttack();
    }

    private void InitializePlayerMovement()
    {
        Utilities.InitializeReferenceOfComponent<PlayerMovement>(gameObject, ref _movement);
        _movement.MoveSpeed = _moveSpeed;
    }

    private void InitializePlayerAttack()
    {
        Utilities.InitializeReferenceOfComponent<PlayerAttack>(gameObject, ref _attack);
        _attack.AttackObject = _attackObject;
        _attack.Chord = _chord;
        _attack.ChordAudioSource = _chordAudioSource;
        _attack.PenaltyCooldown = _penaltyCooldown;
    }
}
