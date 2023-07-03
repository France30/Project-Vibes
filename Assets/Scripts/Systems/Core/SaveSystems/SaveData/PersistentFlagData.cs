using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PersistentFlagData
{
    public string id;
    public bool isTrue;

    public PersistentFlagData(string id, bool isTrue)
    {
        this.id = id;
        this.isTrue = isTrue;
    }
}
