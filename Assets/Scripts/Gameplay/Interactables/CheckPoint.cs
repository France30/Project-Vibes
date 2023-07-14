using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckPoint : Interactable
{
    public override void Interact()
    {
        SaveSystem.SavePlayerData();
        AudioManager.Instance.Play("GameSavedSfx");
        Debug.Log("Game Saved!");
    }
}
