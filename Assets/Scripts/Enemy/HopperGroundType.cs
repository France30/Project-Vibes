using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hopper Enemy", menuName = "Enemy/Ground Enemy/Hopper")]
public class HopperGroundType : GroundEnemyType
{
    private int jumpCount = 0;


    public override bool JumpCondition()
    {
        if(BeatSystemController.Instance.IsBeatPlaying && jumpCount <= 0)
        {
            jumpCount++;
            return true;
        }

        return false;
    }

    public override bool MoveCondition()
    {
        return BeatSystemController.Instance.IsBeatPlaying;
    }

    public override void RegisterEvents()
    {
        BeatSystemController.Instance.OnTickEvent += ResetJumpCount;
    }

    public override void UnregisterEvents()
    {
        if (BeatSystemController.Instance == null) return;

        BeatSystemController.Instance.OnTickEvent -= ResetJumpCount;
    }

    private void ResetJumpCount()
    {
        if (jumpCount > 0)
            jumpCount = 0;
    }
}
