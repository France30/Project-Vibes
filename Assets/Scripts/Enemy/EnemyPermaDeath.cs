using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


[RequireComponent(typeof(EnemyBase))]
public class EnemyPermaDeath : MonoBehaviour
{
    [SerializeField] private string _id = "0";

    private EnemyBase _enemyBase;


    private void Awake()
    {
        _enemyBase = GetComponent<EnemyBase>();

        Application.quitting += ClearEnemySave; //to remove, for testing only
    }

    private void Start()
    {
        LoadEnemyData();        
    }

    private void OnEnable()
    {
        _enemyBase.OnEnemyDeath += SaveEnemyDeath;
    }

    private void OnDisable()
    {
        if (_enemyBase == null) return;

        _enemyBase.OnEnemyDeath -= SaveEnemyDeath;
    }

    private void ClearEnemySave()
    {
        string enemyPath = Application.persistentDataPath + "/enemy" + _id + ".data";
        if (!File.Exists(enemyPath)) return;

        Debug.Log("Enemy Save Deleted");
        File.Delete(enemyPath);
    }

    private void SaveEnemyDeath()
    {
        Debug.Log("Enemy no." + _id + " is dead");
        BinaryFormatter formatter = new BinaryFormatter();
        string enemyPath = Application.persistentDataPath + "/enemy" + _id + ".data";
        FileStream stream = new FileStream(enemyPath, FileMode.Create);

        EnemyPermaDeathData enemyPermaDeathData = new EnemyPermaDeathData(_id, false);
        formatter.Serialize(stream, enemyPermaDeathData);
        stream.Close();
    }

    private void LoadEnemyData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string enemyPath = Application.persistentDataPath + "/enemy" + _id + ".data";
        if (File.Exists(enemyPath))
        {
            FileStream stream = new FileStream(enemyPath, FileMode.Open);

            EnemyPermaDeathData enemyPermaDeath = formatter.Deserialize(stream) as EnemyPermaDeathData;
            gameObject.SetActive(enemyPermaDeath.isAlive);
            stream.Close();
        }
    }
}
