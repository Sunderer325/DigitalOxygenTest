using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThrowAttack", menuName = "Attacks/ThrowAttack")]
public class ThrowAttack : Attack
{
    public Projectile Projectile = default;
    public float Force = 1f;

    public override void Action(Vector2 origin, Vector2 target, Being owner)
    {
        Instantiate(Projectile, origin + target * 0.3f, Quaternion.identity).Init(target * Force, owner);
    }
}
