using UnityEngine;

public class FlyingMeleeEnemy : Enemy
{
    [SerializeField] float amplitude = 1;
    [SerializeField] float frequrency = 2;

    float time;

    protected override void Start()
    {
        base.Start();
        EnemyType = EnemyType.AIR;

        audio.PlayLoop("enemy_fly");
    }

    protected override bool AttackTarget()
    {
        if (base.AttackTarget())
        {
            forcedMovement = true;
            ForcedMovementVelocity = -Velocity;
            return true;
        }
        return false;
    }

    protected override void CollisionsUpdate(){}

    protected override void ForcedMovement()
    {
        ForcedMovementVelocity.y += gravity * Time.deltaTime;
        movement.Move(ForcedMovementVelocity * Time.deltaTime);
    }

    protected override void NormalMovement()
    {
        time += Time.deltaTime;
        float sine = amplitude * (Mathf.Sin(2 * Mathf.PI * frequrency * time));

        Vector2 direction = (target.transform.position - transform.position).normalized;
        Velocity = direction * moveSpeed;

        if (Velocity.x == 0f)
            Velocity.x = sine;
        else
            Velocity.x += Velocity.x * sine * Mathf.Abs(Velocity.normalized.y);

        movement.Move(Velocity * Time.deltaTime);
    }
}
