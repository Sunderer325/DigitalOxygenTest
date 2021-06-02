using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Spear : Projectile
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float smashDelay = 1.5f;
    LineRenderer line;

    bool smashed;
    float comebackTolerance = 0.1f;
    Vector3[] lineVertices = new Vector3[2];

    protected override void Start()
    {
        base.Start();
        line = GetComponent<LineRenderer>();
        line.positionCount = lineVertices.Length;
        line.SetPositions(lineVertices);
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
        StartCoroutine(MoveSpear());
    }

    IEnumerator MoveSpear()
    {
        while (!movement)
            yield return null;

        while (!movement.collisions.any && !smashed) { 
            velocity = initialVelocity * moveSpeed;
            yield return null;
        }

        smashed = true;
        velocity = Vector2.zero;
        movement.collisionMask = 0;
        movement.headCollisionMask = 0;

        yield return new WaitForSeconds(smashDelay);

        while (!IsItBack())
        {
            Vector2 direction = (owner.transform.position - transform.position).normalized;
            velocity = direction * moveSpeed;
            yield return null;
        }

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
