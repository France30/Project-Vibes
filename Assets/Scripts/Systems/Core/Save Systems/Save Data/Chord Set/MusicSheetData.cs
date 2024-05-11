[System.Serializable]
public class MusicSheetData
{
	public string musicSheetSOPath;
	public bool isAddedToChordSet;
	public bool isFound;

	public MusicSheetData(MusicSheet musicSheet)
	{
		this.musicSheetSOPath = "Scriptable Objects/MusicSheetSO/" + musicSheet.musicSheetSO.name;
		this.isAddedToChordSet = musicSheet.isAddedToChordSet;
		this.isFound = musicSheet.isFound;
	}
}
