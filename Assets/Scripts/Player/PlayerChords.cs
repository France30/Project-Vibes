using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChords : MonoBehaviour
{
    private ChordSet[] _chordSets;
    private int _currentChordSet = 0;

    public ChordSet CurrentChordSet { get { return _chordSets[_currentChordSet]; } }


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
        _chordSets = GetComponentsInChildren<ChordSet>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            MoveToNextChordSet();

        if (Input.GetKeyDown(KeyCode.Q))
            MoveToPreviousChordSet();
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
    }
}
