using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FireProjectileDirection
{
    FireHorizontally,
    FireVertically
}

[CreateAssetMenu(fileName = "New Fire Projectile", menuName = "Abilities/Fire Projectile")]
public class FireProjectile : ScriptableObject, IFlyingAttack, IGroundAttack
{
    [SerializeField] private string _id = "Projectile";

    public int damage = 1;
    public float speed = 10f;
    public Sprite sprite;
    public bool isHoming = false;
    public bool canHitGround = false;
    public FireProjectileDirection fireDirection;

    public AbilityType AbilityType { get { return AbilityType.Projectile; } }


    void IAbility.FireProjectile(Transform projectileSpawnPoint)
    {
        Projectile projectile = ObjectPoolManager.Instance.GetPooledObject(_id).GetComponent<Projectile>();
        projectile.SetProjectile(this);

        projectile.transform.position = projectileSpawnPoint.position;
        projectile.transform.rotation = projectileSpawnPoint.rotation;
        projectile.gameObject.SetActive(true);
    }
}