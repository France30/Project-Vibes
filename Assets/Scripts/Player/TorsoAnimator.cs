using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoAnimator : MonoBehaviour
{
    [SerializeField] private GameObject _armLayer;

    public void EnableArmLayer()
    {
        _armLayer.SetActive(true);
    }

    public void DisableArmLayer()
    {
        _armLayer.SetActive(false);
    }
}
