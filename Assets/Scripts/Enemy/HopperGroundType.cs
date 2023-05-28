using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hopper Enemy", menuName = "Enemy/Ground Enemy/Hopper")]
public class HopperGroundType : GroundEnemyType
{
    private int jumpCount = 0;


    public override bool JumpCondition()
    {
        return BeatSystemController.Instance.IsBeatPlaying && jumpCount <= 0;
    }

    public override bool MoveCondition()
    {
        return BeatSystemController.Instance.IsBeatPlaying;
    }

    public override void RegisterEvents()
    {
        _groundEnemy.OnEnemyJumpEvent += OnJump;
        BeatSystemController.Instance.OnTickEvent += ResetJumpCount;
    }

    public override void UnregisterEvents()
    {
        _groundEnemy.OnEnemyJumpEvent -= OnJump;
        BeatSystemController.Instance.OnTickEvent -= ResetJumpCount;
    }

    private void ResetJumpCount()
    {
        if (jumpCount > 0)
            jumpCount = 0;
    }

    private void OnJump()
    {
        if(_rigidbody2D.velocity.y <= FallingThreshold)
            jumpCount++;
    }
}
