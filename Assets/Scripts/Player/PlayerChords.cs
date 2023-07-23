using UnityEngine;

public class PlayerChords : MonoBehaviour
{
    [SerializeField] private bool _loadSavedChordSet = true;

    private PlayerAttack _playerAttack;

    private ChordSet[] _chordSets;
    private int _currentChordSet = 0;
    private int _prevChordSet = 0;

    public ChordSet[] PlayerChordSets { get { return _chordSets; } }
    public ChordSetSO CurrentChordSet { get { return _chordSets[_currentChordSet].ChordSetSO; } }


    public void AddChordSet(ChordSet chordSet)
    {
        chordSet.transform.parent = transform;
        chordSet.transform.SetAsLastSibling();

        _chordSets = GetComponentsInChildren<ChordSet>();
        SaveSystem.SavePlayerChords(this);
    }

    public void AddToChordSet(MusicSheetSO musicSheetSO)
    {
        for(int i = 0; i < _chordSets.Length; i++)
        {
            if (_chordSets[i].ChordSetSO.ChordType != musicSheetSO.ChordType) continue;

            _chordSets[i].AddChordClips(musicSheetSO.ChordClips);
            SaveSystem.SavePlayerChords(this);
            break;
        }
    }

    private void Awake()
    {
        _playerAttack = GameController.Instance.Player.GetComponent<PlayerAttack>();
        _chordSets = GetComponentsInChildren<ChordSet>();

        if (_loadSavedChordSet)
        {
            InitializeChordSetsOnLoad();
        }
    }

    private void Start()
    {
        GameUIManager.Instance.SetSongTitleUI(CurrentChordSet.ID);
    }

    private void InitializeChordSetsOnLoad()
    {
        PlayerChordsData playerChordsData = SaveSystem.LoadPlayerChords();
        if (playerChordsData != null)
        {
            //Destroy any existing chord sets
            for (int i = 0; i < _chordSets.Length; i++)
            {
                Destroy(_chordSets[i].gameObject);
            }
            //Instantiate saved chord sets
            _chordSets = new ChordSet[playerChordsData.chordSetData.Length];
            for (int i = 0; i < playerChordsData.chordSetData.Length; i++)
            {
                GameObject chordSetGO = new GameObject();
                chordSetGO.transform.parent = transform;
                //Disable the gameobject to prevent any functions from getting triggered when attaching components
                chordSetGO.SetActive(false); 
                ChordSet chordSet = chordSetGO.AddComponent<ChordSet>();

                InitializeChordSetValues(chordSet, playerChordsData.chordSetData[i]);
                chordSet.transform.SetAsLastSibling();
                //Enable gameobject to load all components
                chordSetGO.SetActive(true);
                _chordSets[i] = chordSet;
            }
        }
    }

    private void InitializeChordSetValues(ChordSet chordSet, ChordSetData chordSetData)
    {
        ChordSetSO chordSetSO = Resources.Load<ChordSetSO>(chordSetData.chordSetSOPath);
        chordSet.SetChordSetSO(chordSetSO);

        ChordClip[] chordClips = InitializeChordSetSOClips(chordSetData.chordClipData);
        chordSet.ChordSetSO.chordClips = chordClips;
        chordSet.ChordSetSO.time = chordSetData.time;
    }

    private ChordClip[] InitializeChordSetSOClips(ChordClipData[] chordClipData)
    {
        ChordClip[] chordClips = new ChordClip[chordClipData.Length];
        for (int i = 0; i < chordClips.Length; i++)
        {
            ChordClip chordClip = new ChordClip();
            
            AudioClip audioClip = Resources.Load<AudioClip>(chordClipData[i].audioClipPath);
            chordClip.clip = (audioClip == null) ? null : audioClip;

            chordClip.volume = chordClipData[i].volume;
            chordClip.pitch = chordClipData[i].pitch;
            chordClip.beats = chordClipData[i].beats;

            chordClips[i] = chordClip;
        }

        return chordClips;
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
            MoveToNextChordSet();

        if (Input.GetKeyDown(KeyCode.Q))
            MoveToPreviousChordSet();

        if (_prevChordSet != _currentChordSet)
        {
            GameUIManager.Instance.SetSongTitleUI(CurrentChordSet.ID);
            ResetPlayerAttack();
        }
    }

    private void MoveToNextChordSet()
    {
        _currentChordSet = (_currentChordSet < _chordSets.Length - 1) ? _currentChordSet + 1 : 0; 
    }

    private void MoveToPreviousChordSet()
    {
        _currentChordSet = (_currentChordSet > 0) ? _currentChordSet - 1 : _chordSets.Length - 1;
    }

    private void ResetPlayerAttack()
    {
        _prevChordSet = _currentChordSet;

        if (!_playerAttack.enabled) return;

        _playerAttack.enabled = false;
        _playerAttack.enabled = true;
    }
}
