using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckPoint : Interactable
{
    public override void Interact()
    {
        SaveSystem.Save();
        Debug.Log("Game Saved!");
    }
}
