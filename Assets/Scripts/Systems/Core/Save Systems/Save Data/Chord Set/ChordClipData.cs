[System.Serializable]
public class ChordClipData
{
	public string audioClipPath = "";
	public float volume = 0;
	public float pitch = 0;
	public int beats = 0;

	public ChordClipData(ChordClip chordClip)
	{
		if (chordClip.clip == null) return;

		//since we can't directly get the name of the audio clip, we need to convert it into a string
		//unfortunately, using ToString will also include the data type of the object in the string
		//to get around this, we must split the extra string as a workaround
		//we then get the first half of the split as our filename
		string[] audioFile = chordClip.clip.ToString().Split(" ");
		audioClipPath = "Audio/" + audioFile[0];

		volume = chordClip.volume;
		pitch = chordClip.pitch;
		beats = chordClip.beats;
	}
}
