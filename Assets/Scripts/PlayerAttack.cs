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

        foreach (Sound s in chord.sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
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

        Play(chord.sounds[currentNote].name);

        bool isNotePlaying = chord.sounds[currentNote].clip != null;
        attackObject.SetActive(isNotePlaying);
        animator.SetBool("Attack", isNotePlaying);
        
        bool isChordDone = currentNote >= (chord.sounds.Length - 1);
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

    public void Play(string name)
    {
        Sound s = Array.Find(chord.sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound " + name + " not found!");
            return;
        }
        s.source.Play();
    }
}
