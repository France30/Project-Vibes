using UnityEngine;

public static class EnemyUtilities
{
	public static Vector3 FreeMoveTowardsTarget(ref float moveSpeed, Transform transform, Transform target)
	{
		moveSpeed = Mathf.Abs(moveSpeed) * -1; //moveSpeed value must always be negative
		float move = (moveSpeed * Time.fixedDeltaTime) * 3f;
		Vector3 direction = (transform.position - target.position).normalized;
		return direction * move;
	}

	public static Quaternion LookAtTarget(Transform transform, Transform target)
	{
		float offset = 0;
		Vector2 direction = (target.position - transform.position).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		Quaternion rotation = Quaternion.Euler(Vector3.forward * (angle + offset));

		return rotation;
	}

	public static int GetCollisionDirection(Transform transform, Collider2D otherCollider2D)
	{
		bool isCollisionRight = otherCollider2D.transform.position.x > transform.position.x;
		return (isCollisionRight) ? 1 : -1;
	}
}
