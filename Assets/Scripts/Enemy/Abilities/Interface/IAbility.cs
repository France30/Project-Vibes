using UnityEngine;


public enum AbilityType
{
    Projectile,
    Dash
}

public interface IAbility
{
    public AbilityType AbilityType { get; }

    public virtual void FireProjectile(Transform projectileSpawnPoint, int damage = 1) { }
}
