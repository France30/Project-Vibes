using UnityEngine;


public enum FireProjectileDirection
{
    FireHorizontally,
    FireVertically
}

[CreateAssetMenu(fileName = "New Fire Projectile", menuName = "Abilities/Fire Projectile")]
public class FireProjectile : ScriptableObject, IFlyingAttack, IGroundAttack, IBossAttack
{
    [SerializeField] private string _id = "Projectile";

    public float speed = 10f;
    public Sprite sprite;
    public bool isHoming = false;
    public bool canHitGround = false;
    public bool canBeDamaged = false;
    public FireProjectileDirection fireDirection;

    public AbilityType AbilityType { get { return AbilityType.Projectile; } }


    void IAbility.FireProjectile(Transform projectileSpawnPoint, int damage)
    {
        Projectile projectile = ObjectPoolManager.Instance.GetPooledObject(_id).GetComponent<Projectile>();
        projectile.SetProjectile(this, damage);

        projectile.transform.position = projectileSpawnPoint.position;
        projectile.transform.rotation = projectileSpawnPoint.rotation;
        projectile.gameObject.SetActive(true);
    }
}
