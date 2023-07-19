using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerChordsData
{
    public ChordSetData[] chordSetData;

    public PlayerChordsData(ChordSetData[] chordSetData)
    {
        this.chordSetData = chordSetData;
    }
}
