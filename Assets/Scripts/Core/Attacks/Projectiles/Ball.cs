using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : Projectile
{
    [SerializeField, Range(0f, 1f)] float bounciness = 0.5f;
    [SerializeField, Range(0f, 1f)] float friction = 0.8f;
    [SerializeField] float stopDamage = 0.0f;

    bool gravityIsOn;

    // Update is called once per frame
    protected override void Update()
    {
        if (movement.collisions.above || movement.collisions.below)
        {
            velocity.y *= -1 * bounciness;
            velocity.x *= friction;
            gravityIsOn = true;
        }
        else
        {
            //if(gravityIsOn)
                velocity.y += gravity;
        }

        if (movement.collisions.left || movement.collisions.right)
        {
            velocity.x *= -1 * bounciness;
            gravityIsOn = true;
        }

        movement.Move(velocity * Time.deltaTime);
        base.Update();
    }

    protected override void DetectVictim()
    {
        if (velocity.sqrMagnitude > stopDamage)
            base.DetectVictim();
    }
}
