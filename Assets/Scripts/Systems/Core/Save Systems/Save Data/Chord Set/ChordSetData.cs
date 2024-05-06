[System.Serializable]
public class ChordSetData
{
    public string chordSetSOPath;
    public MusicSheetData[] musicSheetData;
    public ChordClipData[] chordClipData;
    public float time;

    public ChordSetData(ChordSet chordSet)
    {
        this.chordSetSOPath = "Scriptable Objects/ChordSetSO/" + chordSet.ChordSetSO.name;

        this.musicSheetData = new MusicSheetData[chordSet.MusicSheets.Length];
        for(int i = 0; i < musicSheetData.Length; i++)
        {
            musicSheetData[i] = new MusicSheetData(chordSet.MusicSheets[i]);
        }

        this.chordClipData = new ChordClipData[chordSet.ChordSetSO.chordClips.Length];
        for(int i = 0; i < chordClipData.Length; i++)
        {
            chordClipData[i] = new ChordClipData(chordSet.ChordSetSO.chordClips[i]);
        }

        this.time = chordSet.ChordSetSO.time;
    }
}
