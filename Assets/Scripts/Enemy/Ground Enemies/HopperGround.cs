
public class HopperGround : GroundEnemy
{
    private int jumpCount = 0;


    protected override bool JumpCondition()
    {
        bool shouldJump = false;

        if(BeatSystemController.Instance.IsBeatPlaying && jumpCount <= 0)
        {
            jumpCount++;
            shouldJump = true;
        }
        else if(!BeatSystemController.Instance.IsBeatPlaying && jumpCount > 0)
        {
            jumpCount = 0;
        }

        return shouldJump;
    }

    protected override bool MoveCondition()
    {
        return BeatSystemController.Instance.IsBeatPlaying;
    }
}
