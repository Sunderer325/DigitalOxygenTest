using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Spyder : Projectile
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float stunningTime = 5f;
    [SerializeField] float webLength = 5f;

    LineRenderer line;
    Vector3[] lineVertices = new Vector3[2];
    Vector3 lastOwnerPos;

    bool isHit;
    GameObject victim;

    public override void Init(Vector2 force, Being owner)
    {
        base.Init(force, owner);
        line = GetComponent<LineRenderer>();
        line.positionCount = lineVertices.Length;
        line.SetPositions(lineVertices);
        lastOwnerPos = owner.transform.position;
    }

    protected override void Update()
    {
        base.Update();
        Vector2 delta = transform.position - lastOwnerPos;
        transform.up = -delta;

        lineVertices[0] = transform.position + (lastOwnerPos - transform.position) * 0.02f;
        if ((lastOwnerPos - transform.position).sqrMagnitude < ((lastOwnerPos - transform.position).normalized * webLength).sqrMagnitude)
            lineVertices[1] = lastOwnerPos;
        else
            lineVertices[1] = (lastOwnerPos - transform.position).normalized * webLength + transform.position;

        line.SetPositions(lineVertices);

        if (!isHit)
        {
            velocity = initialVelocity * moveSpeed;
        }
        else
        {
            transform.position = victim.transform.position;
        }
    }

    protected override void Hit(Collider2D hit)
    {
        if (isHit) return;

        line.enabled = false;
        timeToLive = stunningTime;
        hit.transform.GetComponent<Being>().Stunning(stunningTime);
        isHit = true;
        victim = hit.gameObject;
    }
}
