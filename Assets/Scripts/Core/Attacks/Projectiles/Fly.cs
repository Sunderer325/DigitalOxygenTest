using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Fly : Projectile
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float amplitude = 10f;
    [SerializeField] float frequrency = 1f;
    Enemy closest;
    float phase, time;
  
    protected override void Start()
    {
        base.Start();
        FindClosestTarget();
        if (closest == null)
            StartCoroutine(FindEnemy());
    }
    protected override void Update()
    {
        base.Update();

        time += Time.deltaTime;
        float sine = amplitude * (Mathf.Sin(2 * Mathf.PI * frequrency * time));

        if(closest != null)
        {
            if(closest.gameObject.CompareTag("Untagged"))
                FindClosestTarget();
            
            Vector2 direction = (closest.transform.position - transform.position).normalized;
            velocity = direction * moveSpeed;
            velocity.y += velocity.y * sine;
        }
        else
        {
            velocity.x *= 0.95f;
            velocity.y = sine;
        }


        movement.Move(velocity * Time.deltaTime);
    }

    protected override void Hit(Collider2D hit)
    {
        base.Hit(hit);

        Destroy(gameObject);
    }

    void FindClosestTarget()
    {
        Enemy[] targets = FindObjectsOfType<Enemy>();
        var enemies = targets.Where(c => c.gameObject.CompareTag("Enemy"));

        if (enemies.Count() == 0)
            return;

        closest = enemies.First();
        foreach(Enemy enemy in enemies)
        {
            float nextDst = Vector2.Distance(transform.position, enemy.transform.position);
            float closestDst = Vector2.Distance(transform.position, closest.transform.position);
            if (nextDst < closestDst)
                closest = enemy;
        }
    }

    IEnumerator FindEnemy()
    {
        while(closest == null)
        {
            yield return new WaitForSeconds(3);
            FindClosestTarget();
        }
    }
}
