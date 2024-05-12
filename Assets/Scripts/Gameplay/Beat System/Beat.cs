public enum BeatCooldown
{
	Tick,
	Beat,
	MissedBeat,
}

[System.Serializable]
public class Beat
{
	public int beatCount = 4;
	public float beatSpeed = 1.0f;

	public string tickBGM;
	public string beatBGM;
}
