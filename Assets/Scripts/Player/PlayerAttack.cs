using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject attackObject;
    [SerializeField] private Chord chord;

    private Animator animator;

    private bool isAttacking = false;
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
            if(!BeatSystemController.Instance.IsBeatPlaying)
            {
                AttackNotOnBeat();
                return;
            }

            if(!isAttacking)
                StartCoroutine(PlayAttack());
        }

        if (Input.GetButtonUp("Fire1"))
            currentNote = 0;

    }

    private void AttackNotOnBeat()
    {
        AudioManager.Instance.Play("PlayerMissedBeat");
    }

    private IEnumerator PlayAttack()
    {
        isAttacking = true;

        chord.chordClips[currentNote].source.Play();

        bool isNotePlaying = chord.chordClips[currentNote].clip != null;
        attackObject.SetActive(isNotePlaying);
        animator.SetBool("Attack", isNotePlaying);
        
        bool isSongDone = currentNote >= (chord.chordClips.Length - 1);
        if (!isSongDone)
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
