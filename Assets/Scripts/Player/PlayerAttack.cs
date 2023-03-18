using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject attackObject;
    [SerializeField] private Chord chord;

    private bool isAttacking = false;
    private int currentNote = 0;

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetButton("Fire1") && !isAttacking)
        {
            StartCoroutine(PlayAttack());
        }
    }

    private IEnumerator PlayAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(chord.time);

        attackObject.SetActive(chord.playNote[currentNote]);

        bool isChordDone = currentNote >= (chord.playNote.Count - 1);
        if (Input.GetButton("Fire1") && !isChordDone) currentNote++;
        else currentNote = 0;

        isAttacking = false;
    }
}
