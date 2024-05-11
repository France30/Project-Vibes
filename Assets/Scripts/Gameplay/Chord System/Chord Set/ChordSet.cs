using System.Linq;
using UnityEngine;


public class ChordSet : MonoBehaviour
{
	[SerializeField] private ChordSetSO _chordSetSO;
	[SerializeField] private MusicSheet[] _musicSheets;

	public ChordSetSO ChordSetSO { get { return _chordSetSO; } }
	public MusicSheet[] MusicSheets { get { return _musicSheets; } }


	public void SetChordSetSO(ChordSetSO chordSetSO)
	{
		if (_chordSetSO != null) return;

		_chordSetSO = chordSetSO;
	}

	public void SetMusicSheets(MusicSheet[] musicSheets)
	{
		if (_musicSheets != null) return;

		_musicSheets = musicSheets;
	}

	public void AddChordClips(ChordClip[] otherChordClips)
	{
		ChordClip[] chordClips = _chordSetSO.chordClips.Concat<ChordClip>(otherChordClips).ToArray();
		_chordSetSO.chordClips = chordClips;

		InitializeChordAudioSource(_chordSetSO);
	}

	private void Awake()
	{
		InitializeChordAudioSource(_chordSetSO);
	}

	private void InitializeChordAudioSource(ChordSetSO chordSO)
	{
		ChordClip[] chordClips = chordSO.chordClips;
		for (int i = 0; i < chordClips.Length; i++)
		{
			if (chordClips[i].source != null) continue;

			chordClips[i].source = gameObject.AddComponent<AudioSource>();
			chordClips[i].source.clip = chordClips[i].clip;

			chordClips[i].source.volume = chordClips[i].volume;
			chordClips[i].source.pitch = chordClips[i].pitch;
		}
	}
}
