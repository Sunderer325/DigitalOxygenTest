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

        if (movement.Collisions.Any && velocity.sqrMagnitude > stopDamage)
        {
            //float volume = Vector2.Dot(movement.Collisions.Normal, velocity);
            float volume = velocity.sqrMagnitude / initialVelocity.sqrMagnitude;
            audio.Play("ball_hit", volume);
        }

        if (movement.Collisions.Above || movement.Collisions.Below)
        {
            velocity.y *= -1 * bounciness;
            velocity.x *= friction;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (movement.Collisions.Left || movement.Collisions.Right)
        {
            velocity.x *= -1 * bounciness;
        }
    }

    protected override void DetectVictim()
    {
        if (velocity.sqrMagnitude > stopDamage && movement.Collisions.Any)
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
