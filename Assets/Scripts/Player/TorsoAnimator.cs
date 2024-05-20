using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoAnimator : MonoBehaviour
{
    [Header("Body Layers")]
    [SerializeField] private GameObject _armLayer;

    [Header("Head Transforms")]
    [SerializeField] private Transform _headTransform;
    [SerializeField] private Transform _initialHeadTrans;
    [SerializeField] private Transform _jumpTransHead;
    [SerializeField] private Transform _runTransHead;
    [SerializeField] private Transform _fallTransHead;
    [SerializeField] private Transform _hurtTransHead;

    public void EnableArmLayer()
    {
        _armLayer.SetActive(true);
    }

    public void DisableArmLayer()
    {
        _armLayer.SetActive(false);
    }

    public void SetHeadJump()
    {
        _headTransform.position = _jumpTransHead.position;
        _headTransform.rotation = _jumpTransHead.rotation;
    }

    public void SetHeadRun()
    {
        _headTransform.position = _runTransHead.position;
        _headTransform.rotation = _runTransHead.rotation;
    }

    public void SetHeadFall()
    {
        _headTransform.position = _fallTransHead.position;
        _headTransform.rotation = _fallTransHead.rotation;
    }

    public void SetHeadHurt()
    {
        _headTransform.position = _hurtTransHead.position;
        _headTransform.rotation = _hurtTransHead.rotation;
    }

    public void SetHeadInitial()
    {
        _headTransform.position = _initialHeadTrans.position;
        _headTransform.rotation = _initialHeadTrans.rotation;
    }
}
