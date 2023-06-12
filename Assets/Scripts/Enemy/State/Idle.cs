using UnityEngine;


public class Idle : State
{
    [SerializeField] private float _idleTime = 3f;


    private float _currentTime = 0f;


    public override void PerformState()
    {
        base.PerformState();

        if (_currentTime < _idleTime)
            _currentTime += Time.fixedDeltaTime;
    }


    private void LateUpdate()
    {
        //Reset timer
        if (_enemyBase.CurrentState != this) 
            _currentTime = 0;
    }
}
