using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="MeleeAttack", menuName = "Attacks/MeleeAttack")]
public class MeleeAttack : Attack
{
    [SerializeField] float knockbackY = 0.9f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float damageRadius = 0.5f;
    [SerializeField] protected float knockbackForce = 5f;
    public float DamageRadius => damageRadius;

    Vector2 origin;
    public override void Action(Vector2 origin, Vector2 target, Being owner)
    {
        LayerMask layermask = 0;
        if (owner.GetBeingType == BeingType.ENEMY)
            layermask = LayerMask.GetMask("Player");
        else if (owner.GetBeingType == BeingType.PLAYER)
            layermask = LayerMask.GetMask("Enemy");

        Collider2D hit = Physics2D.OverlapCircle(origin, damageRadius, layermask);
        if (hit)
        {
            if (!IsCooldownEnd())
                return;

            Vector2 knockbackDirection = ((Vector2)hit.transform.position - origin).normalized;
            if (knockbackDirection.y < knockbackY)
                knockbackDirection.y = knockbackY;
            hit.transform.GetComponent<Being>().GetDamage(damage, knockbackDirection * knockbackForce);
        }
    }
}
