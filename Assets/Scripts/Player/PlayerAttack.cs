using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject attackObject;
    [SerializeField] private Chord chord;
    [SerializeField] private float missedCooldown = 3.0f;

    private Animator animator;

    private bool isAttacking = false;
    private bool didPlayerMissBeat = false;
    private int currentNote = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        foreach (ChordClip c in chord.chordClips)
        {
            c.source = gameObject.AddComponent<AudioSource>();
            c.source.clip = c.clip;

            c.source.volume = c.volume;
            c.source.pitch = c.pitch;
        } 
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (didPlayerMissBeat) return;

            if(!BeatSystemController.Instance.IsBeatPlaying)
            {
                StartCoroutine(AttackNotOnBeat());
                return;
            }

            if(!isAttacking)
                StartCoroutine(PlayAttack());
        }

        if (Input.GetButtonUp("Fire1"))
            currentNote = 0;

    }

    private IEnumerator AttackNotOnBeat()
    {
        didPlayerMissBeat = true;
        AudioManager.Instance.Play("PlayerMissedBeat");

        yield return new WaitForSeconds(missedCooldown);

        didPlayerMissBeat = false;
    }

    private IEnumerator PlayAttack()
    {
        isAttacking = true;

        chord.chordClips[currentNote].source.Play();

        bool isNotePlaying = chord.chordClips[currentNote].clip != null;
        attackObject.SetActive(isNotePlaying);
        animator.SetBool("Attack", isNotePlaying);
        
        bool isChordDone = currentNote >= (chord.chordClips.Length - 1);
        if (!isChordDone)
            currentNote++;
        else
            currentNote = 0;

        yield return new WaitForSeconds(chord.time);

        attackObject.SetActive(false);
        animator.SetBool("Attack", false);
        isAttacking = false;

        if (Input.GetButton("Fire1") && currentNote != 0) //continue chord progression
            StartCoroutine(PlayAttack());
    }
}
