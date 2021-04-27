using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    GameObject[] targets;
    protected override void Start()
    {
        base.Start();
        type = EnemyType.AIR;
        targets = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (isDie) return;

        attack.Action(transform.position, Vector2.zero, beingType);

        Vector2 direction = FindClosestTarget().normalized;
        velocity = direction * moveSpeed;
        movement.Move(velocity * Time.deltaTime);
    }

    Vector2 FindClosestTarget()
    {
        Vector2 closest = targets[0].transform.position - transform.position;
        foreach(GameObject t in targets)
        {
            Vector2 distance = t.transform.position - transform.position;
            if (distance.magnitude < closest.magnitude)
                closest = distance;
        }
        return closest;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
