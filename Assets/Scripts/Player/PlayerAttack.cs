using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject attackObject;
    [SerializeField] private Chord chord;
    [SerializeField] private float penaltyCooldown = 3.0f;

    private AttackObjectController attackObjectController;
    private Animator animator;

    private bool isAttacking = false;
    private bool didPlayerMissBeat = false;
    private int currentNote = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        attackObjectController = attackObject.GetComponent<AttackObjectController>();

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

        yield return new WaitForSeconds(penaltyCooldown);

        didPlayerMissBeat = false;
    }

    private IEnumerator PlayAttack()
    {
        isAttacking = true;
        chord.chordClips[currentNote].source.Play();

        bool isChordPlaying = chord.chordClips[currentNote].clip != null;
        SetAttackBehaviour(isChordPlaying);
        CheckIfChordIsHalfChord();

        CheckIfSongDone();

        yield return new WaitForSeconds(chord.time);

        SetAttackBehaviour(false);
        isAttacking = false;

        if (Input.GetButton("Fire1") && currentNote != 0) //continue chord progression
            StartCoroutine(PlayAttack());
    }

    private void SetAttackBehaviour(bool value)
    {
        attackObject.SetActive(value);
        animator.SetBool("Attack", value);
    }

    private void CheckIfChordIsHalfChord()
    {
        bool isPlayingHalfChord = chord.chordClips[currentNote].IsHalfChord;
        if (!isPlayingHalfChord) return;

        float animationSpeedMultiplier = 2.2f;
        attackObjectController.AnimationSpeed *= animationSpeedMultiplier;
    }

    private void CheckIfSongDone()
    {
        bool isSongDone = currentNote >= (chord.chordClips.Length - 1);
        if (!isSongDone)
            currentNote++;
        else
            currentNote = 0;
    }
}
