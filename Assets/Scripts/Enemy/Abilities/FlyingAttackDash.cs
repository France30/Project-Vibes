using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Flying Attack Dash", menuName = "Abilities/Flying Attack Dash")]
public class FlyingAttackDash : ScriptableObject, IFlyingAttack
{
    [Range(0, 200)] public float speedMultiplier; //multiplies speed when attacking

    public AbilityType AbilityType { get { return AbilityType.Dash; } }


    Vector3 IFlyingAttack.ApplyAttackVelocity(float moveSpeed, Transform transform)
    {
        if (GameController.Instance.Player == null) return Vector3.zero;

        moveSpeed = Mathf.Abs(moveSpeed) * -1; //moveSpeed value must always be negative
        float attackMove = moveSpeed * speedMultiplier;
        attackMove = (attackMove * Time.fixedDeltaTime) * 3f;

        Vector3 attackDirection = (transform.position - GameController.Instance.Player.transform.position).normalized;

        return attackDirection * attackMove;
    }
}
