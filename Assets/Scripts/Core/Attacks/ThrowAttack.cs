using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThrowAttack", menuName = "Attacks/ThrowAttack")]
public class ThrowAttack : Attack
{
    [SerializeField] Projectile projectile = default;
    [SerializeField] float force = 1f;

    public override void Action(Vector2 origin, Vector2 target, Being owner)
    {
        Instantiate(projectile, origin + target * 0.3f, Quaternion.identity).Init(target * force, owner);
    }
}
