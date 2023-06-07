using UnityEngine;

public static class EnemyUtilities
{
    public static Quaternion LookAtPlayer(Transform transform)
    {
        if (GameController.Instance.Player == null) return Quaternion.identity;

        float offset = 0;
        Vector2 direction = (GameController.Instance.Player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(Vector3.forward * (angle + offset));

        return rotation;
    }
}
