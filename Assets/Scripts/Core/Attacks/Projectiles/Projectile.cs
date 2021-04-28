using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Movement))]
public class Projectile : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] protected float knockbackForce = 5f;
    [SerializeField] protected float gravity = -9.8f;
    [SerializeField] bool destroyOnTTL = true;
    [SerializeField] float timeToLive = 5f;

    protected Vector2 velocity;
    Vector2 initialVelocity;
    BeingType ownerType;

    Vector2 damageBox;
    LayerMask damageMask;

    protected Movement movement;
    BoxCollider2D collider;

    private void Start()
    {
        movement = GetComponent<Movement>();
        collider = GetComponent<BoxCollider2D>();
        damageBox = new Vector2(movement.GetSkinWidth + collider.bounds.size.x, movement.GetSkinWidth + collider.bounds.size.y);
    }

    protected virtual void Update()
    {
        if (destroyOnTTL)
        {
            timeToLive -= Time.deltaTime;
            if (timeToLive < 0f)
                Destroy(gameObject);
        }
        DetectVictim();
    }

    public void Init(Vector2 force, BeingType ownerType)
    {
        AddForce(force);
        this.ownerType = ownerType;

        if (ownerType == BeingType.ENEMY)
            damageMask = LayerMask.GetMask("Player");
        else if (ownerType == BeingType.PLAYER)
            damageMask = LayerMask.GetMask("Enemy");
    }

    protected virtual void DetectVictim()
    {

        Collider2D hit = Physics2D.OverlapBox(transform.position, damageBox, 0,  damageMask);
        if (hit)
        {
            Vector2 knockbackDirection = hit.transform.position - transform.position;
            float mulDesiredForce = velocity.sqrMagnitude / initialVelocity.sqrMagnitude;
            Vector2 knockback = knockbackDirection * (knockbackForce * mulDesiredForce);
            hit.transform.GetComponent<Being>().GetDamage(damage, knockback);
        }
    }

    private void AddForce(Vector2 force)
    {
        initialVelocity = force;
        velocity.x += force.x;
        velocity.y += force.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, damageBox);
    }
}
