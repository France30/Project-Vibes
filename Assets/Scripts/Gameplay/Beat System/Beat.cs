public enum BeatCooldown
{
	Base,
	MissedBeat,
}

[System.Serializable]
public class Beat
{
	public int beatCount = 4;
	public float beatSpeed = 1.0f;
	public float openingOffset = 1.0f;

	public string tickBGM;
	public string beatBGM;
}
