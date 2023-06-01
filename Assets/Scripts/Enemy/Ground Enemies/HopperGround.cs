using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopperGround : GroundEnemy
{
    private int jumpCount = 0;


    protected override bool JumpCondition()
    {
        if(BeatSystemController.Instance.IsBeatPlaying && jumpCount <= 0)
        {
            jumpCount++;
            return true;
        }

        return false;
    }

    protected override bool MoveCondition()
    {
        return BeatSystemController.Instance.IsBeatPlaying;
    }

    private void OnEnable()
    {
        BeatSystemController.Instance.OnTickEvent += ResetJumpCount;
    }

    private void OnDisable()
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
