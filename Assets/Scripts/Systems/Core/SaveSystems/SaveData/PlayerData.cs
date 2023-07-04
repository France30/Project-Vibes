using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int currentLevelSelect;
    public float[] playerPosition = new float[2];

    public PlayerData(int currentLevelIndex, Vector2 currentPlayerPosition)
    {
        currentLevelSelect = currentLevelIndex;
        playerPosition[0] = currentPlayerPosition.x;
        playerPosition[1] = currentPlayerPosition.y;
    }
}
