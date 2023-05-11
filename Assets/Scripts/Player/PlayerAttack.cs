using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject _attackObject;
    [SerializeField] private Chord _chord;
    [SerializeField] private float _penaltyCooldown = 3.0f;

    private AttackObjectController _attackObjectController;
    private Animator _animator;

    private bool _isAttackCoroutineRunning = false;
    private bool _didPlayerMissBeat = false;
    private int _currentChord = 0;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _attackObjectController = _attackObject.GetComponent<AttackObjectController>();

        foreach (ChordClip c in _chord.chordClips)
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
            if (_didPlayerMissBeat) return;

            if(!BeatSystemController.Instance.IsBeatPlaying)
            {
                StartCoroutine(AttackNotOnBeat());
                return;
            }

            if(!_isAttackCoroutineRunning)
                StartCoroutine(PlayAttack());
        }

        if (Input.GetButtonUp("Fire1"))
            _currentChord = 0;
    }

    private IEnumerator AttackNotOnBeat()
    {
        _didPlayerMissBeat = true;
        AudioManager.Instance.Play("PlayerMissedBeat");

        yield return new WaitForSeconds(_penaltyCooldown);

        _didPlayerMissBeat = false;
    }

    private IEnumerator PlayAttack()
    {
        _isAttackCoroutineRunning = true;

        _chord.chordClips[_currentChord].source.Play();

        bool isChordPlaying = _chord.chordClips[_currentChord].clip != null;
        SetAttackBehaviour(isChordPlaying);

        if(isChordPlaying) CheckIfChordIsHalfChord();

        CheckIfSongDone();

        yield return new WaitForSeconds(_chord.time);

        SetAttackBehaviour(false);

        _isAttackCoroutineRunning = false;

        if (Input.GetButton("Fire1") && _currentChord != 0) //continue chord progression
            StartCoroutine(PlayAttack());
    }

    private void SetAttackBehaviour(bool value)
    {
        _attackObject.SetActive(value);
        _animator.SetBool("Attack", value);
    }

    private void CheckIfChordIsHalfChord()
    {
        bool isPlayingHalfChord = _chord.chordClips[_currentChord].IsHalfChord;
        if (!isPlayingHalfChord) return;

        //increase attack hitbox speed if chord is half chord
        //reset attack hitbox scale twice before disabling the attack hitbox game object
        float animationSpeedMultiplier = 2f;
        _attackObjectController.AnimationSpeed *= animationSpeedMultiplier;
        _attackObjectController.HitboxScaleResetCounter = 2;
    }

    private void CheckIfSongDone()
    {
        bool isSongDone = _currentChord >= (_chord.chordClips.Length - 1);
        if (!isSongDone)
            _currentChord++;
        else
            _currentChord = 0;
    }
}
