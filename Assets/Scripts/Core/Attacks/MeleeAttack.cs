using UnityEngine;

[CreateAssetMenu(fileName ="MeleeAttack", menuName = "Attacks/MeleeAttack")]
public class MeleeAttack : Attack
{
    [SerializeField] float knockbackY = 0.9f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float damageDiameter = 1f;
    [SerializeField] protected float knockbackForce = 5f;
    public float DamageRadius => damageDiameter / 2;

    Vector2 origin;
    public override void Action(Vector2 origin, Vector2 target, Being owner)
    {
        LayerMask layermask = 0;
        if (owner.BeingType == BeingType.ENEMY)
            layermask = LayerMask.GetMask("Player");
        else if (owner.BeingType == BeingType.PLAYER)
            layermask = LayerMask.GetMask("Enemy");

        Collider2D hit = Physics2D.OverlapCircle(origin, DamageRadius, layermask);
        if (hit)
        {
            Vector2 knockbackDirection = ((Vector2)hit.transform.position - origin).normalized;
            if (knockbackDirection.y < knockbackY)
                knockbackDirection.y = knockbackY;
            hit.transform.GetComponent<Being>().GetDamage(damage, knockbackDirection * knockbackForce);
        }
    }
}
