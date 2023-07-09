using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChords : MonoBehaviour
{
    private PlayerAttack _playerAttack;

    private ChordSet[] _chordSets;
    private int _currentChordSet = 0;
    private int _prevChordSet = 0;

    public ChordSetSO CurrentChordSet { get { return _chordSets[_currentChordSet].ChordSetSO; } }


    public void AddChordSet(ChordSet chordSet)
    {
        chordSet.transform.parent = transform;
        chordSet.transform.SetAsLastSibling();

        _chordSets = GetComponentsInChildren<ChordSet>();
    }

    public void AddToChordSet(MusicSheetSO musicSheetSO)
    {
        for(int i = 0; i < _chordSets.Length; i++)
        {
            if (_chordSets[i].ChordSetSO.ChordType != musicSheetSO.ChordType) continue;

            _chordSets[i].AddChordClips(musicSheetSO.ChordClips);
            break;
        }
    }

    private void Awake()
    {
        _playerAttack = GameController.Instance.Player.GetComponent<PlayerAttack>();
        _chordSets = GetComponentsInChildren<ChordSet>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            MoveToNextChordSet();

        if (Input.GetKeyDown(KeyCode.Q))
            MoveToPreviousChordSet();

        if (_prevChordSet != _currentChordSet)
            ResetPlayerAttack();
    }

    private void MoveToNextChordSet()
    {
        _currentChordSet++;

        if (_currentChordSet > _chordSets.Length - 1)
            _currentChordSet = 0;
    }

    private void MoveToPreviousChordSet()
    {
        _currentChordSet--;

        if (_currentChordSet < 0)
            _currentChordSet = _chordSets.Length - 1;

    private void ResetPlayerAttack()
    {
        _prevChordSet = _currentChordSet;

        if (!_playerAttack.enabled) return;

        _playerAttack.enabled = false;
        _playerAttack.enabled = true;
    }
}
