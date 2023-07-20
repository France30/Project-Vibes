using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class SaveSystem
{
    private static readonly BinaryFormatter formatter = new BinaryFormatter();


    public static void ClearAllSaveData()
    {
        string[] dataPath = Directory.GetFiles(Application.persistentDataPath + "/","*.data");

        for (int i = 0; i < dataPath.Length; i++)
        {
            File.Delete(dataPath[i]);
        }
    }

    public static void ClearSavedPlayerPositionInLevel(int level)
    {
        PlayerData loadData = LoadPlayerData();
        if (loadData == null || loadData.currentLevelSelect != level) return;

        string playerPath = Application.persistentDataPath + "/player.data";
        FileStream stream = new FileStream(playerPath, FileMode.Create);

        loadData = new PlayerData(level, Vector2.zero);

        formatter.Serialize(stream, loadData);
        stream.Close();
    }

    public static bool IsSaveFileFound()
    {
        string[] dataPath = Directory.GetFiles(Application.persistentDataPath + "/", "*.data");

        return dataPath.Length > 0;
    }

    public static void SavePlayerData()
    {
        string playerPath = Application.persistentDataPath + "/player.data";
        FileStream stream = new FileStream(playerPath, FileMode.Create);

        //player data
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        Transform player = GameController.Instance.Player.transform;
        PlayerData playerData = new PlayerData(currentLevel, player.position);

        formatter.Serialize(stream, playerData);
        stream.Close();
    }

    public static void SaveUnlockedLevels()
    {
        string unlockedLevelsPath = Application.persistentDataPath + "/unlockedLevels.data";
        FileStream stream = new FileStream(unlockedLevelsPath, FileMode.Create);

        List<int> unlockedLevelsData = LevelManager.Instance.LevelsUnlocked;
        formatter.Serialize(stream, unlockedLevelsData);
        stream.Close();
    }

    public static void SavePlayerChords(PlayerChords playerChords)
    {
        string playerChordsPath = Application.persistentDataPath + "/playerChords.data";
        FileStream stream = new FileStream(playerChordsPath, FileMode.Create);

        ChordSetData[] chordSetDatas = new ChordSetData[playerChords.PlayerChordSets.Length];

        for(int i = 0; i < chordSetDatas.Length; i++)
        {
            chordSetDatas[i] = new ChordSetData(playerChords.PlayerChordSets[i]);
        }

        PlayerChordsData playerChordsData = new PlayerChordsData(chordSetDatas);

        formatter.Serialize(stream, playerChordsData);
        stream.Close();
    }

    public static PlayerData LoadPlayerData()
    {
        PlayerData playerData = null;

        string playerPath = Application.persistentDataPath + "/player.data";
        if (File.Exists(playerPath))
        {
            FileStream stream = new FileStream(playerPath, FileMode.Open);

            playerData = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
        }
        else
        {
            Debug.Log("Player Data not found in " + playerPath + "!");
        }

        return playerData;
    }

    public static PlayerChordsData LoadPlayerChords()
    {
        PlayerChordsData playerChordsData = null;

        string playerChordsPath = Application.persistentDataPath + "/playerChords.data";
        if(File.Exists(playerChordsPath))
        {
            FileStream stream = new FileStream(playerChordsPath, FileMode.Open);

            playerChordsData = formatter.Deserialize(stream) as PlayerChordsData;
            stream.Close();
        }
        else
        {
            Debug.Log("Player Chords Data not found in " + playerChordsPath + "!");
        }

        return playerChordsData;
    }

    public static List<int> LoadUnlockedLevels()
    {
        List<int> unlockedLevelsData = new List<int>();
        string unlockedLevelsPath = Application.persistentDataPath + "/unlockedLevels.data";
        if (File.Exists(unlockedLevelsPath))
        {
            FileStream stream = new FileStream(unlockedLevelsPath, FileMode.Open);

            unlockedLevelsData = formatter.Deserialize(stream) as List<int>;
            stream.Close();
        }

        return unlockedLevelsData;
    }
}
