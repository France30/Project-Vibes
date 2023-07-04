using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;


public static class SaveSystem
{
    private static readonly BinaryFormatter formatter = new BinaryFormatter();


    public static void ClearAllSaveData()
    {
        string[] dataPath = Directory.GetFiles(Application.persistentDataPath + "*.data");

        for (int i = 0; i < dataPath.Length; i++)
        {
            File.Delete(dataPath[i]);
        }
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
            Debug.Log("Player Data not found in" + playerPath + "!");
        }

        return playerData;
    }
}
