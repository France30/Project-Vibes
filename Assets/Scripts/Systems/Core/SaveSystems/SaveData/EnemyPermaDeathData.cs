using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyPermaDeathData
{
    public string id;
    public bool isAlive;

    public EnemyPermaDeathData(string id, bool isAlive)
    {
        this.id = id;
        this.isAlive = isAlive;
    }
}
