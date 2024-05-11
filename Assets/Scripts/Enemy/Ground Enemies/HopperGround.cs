
public class HopperGround : GroundEnemy
{
	private int jumpCount = 0;


	public override void OnLanding()
	{
		base.OnLanding();
		_animator.SetBool("Jump", false);
	}

	protected override bool JumpCondition()
	{
		bool shouldJump = false;

		if(BeatSystemController.Instance.IsBeatPlaying && jumpCount <= 0)
		{
			jumpCount++;
			shouldJump = true;
			_animator.SetBool("Jump", true);
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
