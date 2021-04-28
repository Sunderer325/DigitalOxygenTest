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

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (isDie) return;

        GameObject target = attack.Action(transform.position, Vector2.zero, beingType);
        if (target)
        {
            //velocity = new Vector2(velocity.x, velocity.y * -1) * 5;
            forcedMovement = true;
            forcedMovementVelocity = new Vector2(velocity.x * -1, velocity.y * -1);
        }
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
