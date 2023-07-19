using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChordSetData
{
    public string chordSetSOPath;
    public ChordClipData[] chordClipData;
    public float time;

    public ChordSetData(ChordSet chordSet)
    {
        this.chordSetSOPath = "Scriptable Objects/ChordSetSO/" + chordSet.ChordSetSO.name;

        this.chordClipData = new ChordClipData[chordSet.ChordSetSO.chordClips.Length];
        for(int i = 0; i < chordClipData.Length; i++)
        {
            chordClipData[i] = new ChordClipData(chordSet.ChordSetSO.chordClips[i]);
        }

        this.time = chordSet.ChordSetSO.time;
    }
}
