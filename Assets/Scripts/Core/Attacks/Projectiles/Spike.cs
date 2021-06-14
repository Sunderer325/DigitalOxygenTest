using UnityEngine;

public class Spike : Projectile
{
    [SerializeField] float moveSpeed = 5f;

    bool hit;

    public override void Init(Vector2 force, Being owner)
    {
        base.Init(force, owner);
        transform.rotation = Quaternion.AngleAxis(180 + Mathf.Atan2(force.x, force.y) * Mathf.Rad2Deg, Vector3.back);
    }

    protected override void Update()
    {
        base.Update();
        if (hit)
            return;

        velocity = initialVelocity * moveSpeed;

        if (movement.Collisions.Any )
        {
            hit = true;
            velocity = Vector2.zero;
            audio.Play("spike_hit");
        }
    }

    protected override void DetectVictim()
    {
        if(velocity.magnitude > 1f)
            base.DetectVictim();
    }
}
