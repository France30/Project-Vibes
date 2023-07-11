using UnityEngine;

public interface IFlyingAttack: IAbility
{
    public virtual Vector3 ApplyAttackVelocity(float moveSpeed, Transform transform)
    {
        return Vector3.zero;
    }
}
