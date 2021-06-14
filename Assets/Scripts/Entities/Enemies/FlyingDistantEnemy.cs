using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDistantEnemy : Enemy
{
    [SerializeField] float distanceFromTarget = 5f;

    protected override void Start()
    {
        base.Start();
        EnemyType = EnemyType.AIR;

        audio.PlayLoop("enemy_fly");
    }

    protected override bool AttackTarget()
    {
        if (!attack.IsCooldownEnd() || stunning)
            return false;

        if (Vector2.Distance(transform.position, target.transform.position) <= distanceFromTarget)
        {
            attack.Action(transform.position, (target.transform.position - transform.position).normalized, this);
            audio.Play("enemy_spit");
            return true;
        }
        return false;
    }
    protected override void CollisionsUpdate() { }

    protected override void ForcedMovement()
    {
        ForcedMovementVelocity.y += gravity * Time.deltaTime;
        movement.Move(ForcedMovementVelocity * Time.deltaTime);
    }

    protected override void NormalMovement()
    {
        float distance = Vector2.Distance(transform.position, target.transform.position);
        if(distance >= distanceFromTarget)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            Velocity = direction * moveSpeed;
        }
        else if(distance <= distanceFromTarget / 2)
        {
            Vector2 direction = (transform.position - target.transform.position).normalized;
            Velocity = direction * moveSpeed / 2;
        }
        else                                                                                  
        {
            Velocity *= 0.7f;
        }

        movement.Move(Velocity * Time.deltaTime);
    }
}
