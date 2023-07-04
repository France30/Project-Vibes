using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckPoint : Interactable
{
    public override void Interact()
    {
        SaveSystem.SavePlayerData();
        Debug.Log("Game Saved!");
    }
}
