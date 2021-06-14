using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Spear : Projectile
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float smashDelay = 1.5f;
    [SerializeField] float maxFloatDistance = 7f;
    LineRenderer line;

    bool smashed;
    float comebackTolerance = 0.5f;
    Vector3[] lineVertices = new Vector3[2];

    protected override void Start()
    {
        base.Start();
        line = GetComponent<LineRenderer>();
        line.positionCount = lineVertices.Length;
        line.SetPositions(lineVertices);
        StartCoroutine(MoveSpear());
    }

    protected override void Update()
    {
        base.Update();
        Vector2 delta = transform.position - owner.transform.position;
        transform.up = -delta;
        lineVertices[0] = owner.transform.position;
        lineVertices[1] = transform.position;
        line.SetPositions(lineVertices);
    }

    public override void Init(Vector2 force, Being owner)
    {
        base.Init(force, owner);
    }

    IEnumerator MoveSpear()
    {
        while (!movement)
            yield return null;

        audio.Play("hook_float");
        while (!movement.Collisions.Any && !smashed) { 
            velocity = initialVelocity * moveSpeed;
            if (Vector2.Distance(owner.transform.position, transform.position) > maxFloatDistance)
                smashed = true;

            yield return null;
        }

        audio.Stop();
        audio.Play("hook_smash");

        smashed = true;
        velocity = Vector2.zero;

        yield return new WaitForSeconds(smashDelay);

        audio.Play("hook_float");
        movement.CollisionMask = 0;
        movement.HeadCollisionMask = 0;

        while (!IsItBack())
        {
            Vector2 direction = (owner.transform.position - transform.position).normalized;
            velocity = direction * moveSpeed;
            yield return null;
        }

        audio.Stop();
        Destroy(gameObject);
    }

    bool IsItBack()
    {
        if (transform.position.x > owner.transform.position.x - comebackTolerance 
            && transform.position.x < owner.transform.position.x + comebackTolerance
            && transform.position.y > owner.transform.position.y - comebackTolerance 
            && transform.position.y < owner.transform.position.y + comebackTolerance)
        {
            return true;
        }
        return false;
    }
}
