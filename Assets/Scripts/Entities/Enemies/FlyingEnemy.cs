using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
        type = EnemyType.AIR;
    }

    protected override bool AttackTarget()
    {
        if (base.AttackTarget())
        {
            forcedMovement = true;
            forcedMovementVelocity = -velocity;
            return true;
        }
        return false;
    }

    protected override void CollisionsUpdate(){}

    protected override void ForcedMovement()
    {
        forcedMovementVelocity.y += gravity * Time.deltaTime;
        movement.Move(forcedMovementVelocity * Time.deltaTime);
    }

    protected override void NormalMovement()
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        velocity = direction * moveSpeed;
        movement.Move(velocity * Time.deltaTime);
    }
}
