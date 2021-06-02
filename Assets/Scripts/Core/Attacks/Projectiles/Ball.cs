using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : Projectile
{
    [SerializeField, Range(0f, 1f)] float bounciness = 0.5f;
    [SerializeField, Range(0f, 1f)] float friction = 0.8f;
    [SerializeField] float stopDamage = 0.0f;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (movement.collisions.above || movement.collisions.below)
        {
            velocity.y *= -1 * bounciness;
            velocity.x *= friction;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (movement.collisions.left || movement.collisions.right)
        {
            velocity.x *= -1 * bounciness;
        }
    }

    protected override void DetectVictim()
    {
        if (velocity.sqrMagnitude > stopDamage && movement.collisions.any)
            //if(movement.collisions.target.gameObject.CompareTag("Enemy"))
                base.DetectVictim();
    }

    protected override void Hit(Collider2D hit)
    {
        Vector2 knockbackDirection = hit.transform.position - transform.position;
        float mulDesiredForce = velocity.sqrMagnitude / initialVelocity.sqrMagnitude;
        Vector2 knockback = knockbackDirection * (knockbackForce * mulDesiredForce);

        hit.transform.GetComponent<Being>().GetDamage(damage, knockback);
    }
}
