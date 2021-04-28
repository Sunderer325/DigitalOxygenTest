using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BiteAttack")]
public class BiteAttack : Attack
{
    [SerializeField] float biteRadius = 1f;
    [SerializeField] float knockbackY = 0.9f;

    Vector2 origin;
    public override GameObject Action(Vector2 origin, Vector2 target, BeingType beingType)
    {
        LayerMask layermask = 0;
        if (beingType == BeingType.ENEMY)
            layermask = LayerMask.GetMask("Player");
        else if (beingType == BeingType.PLAYER)
            layermask = LayerMask.GetMask("Enemy");

        Collider2D hit = Physics2D.OverlapCircle(origin, biteRadius, layermask);
        if (hit)
        {
            if (!IsCooldownEnd())
                return null;

            Vector2 knockbackDirection = ((Vector2)hit.transform.position - origin).normalized;
            if (knockbackDirection.y < knockbackY)
                knockbackDirection.y = knockbackY;
            hit.transform.GetComponent<Being>().GetDamage(damage, knockbackDirection * knockbackForce);
            return hit.gameObject;
        }
        return null;
    }
}
