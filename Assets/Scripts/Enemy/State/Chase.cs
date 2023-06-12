using UnityEngine;

public class Chase : State
{

    private Transform _player;

    public override void PerformState()
    {
        base.PerformState();
        _enemyBase.MoveToTargetDirection(_player);

    }

    private void Start()
    {
        _player = GameController.Instance.Player.transform;
    }
}
