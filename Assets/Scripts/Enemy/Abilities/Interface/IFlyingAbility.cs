using UnityEngine;

public interface IFlyingAbility: IAbility
{
	public virtual Vector3 ApplyAttackVelocity(float moveSpeed, Transform transform)
	{
		return Vector3.zero;
	}
}
