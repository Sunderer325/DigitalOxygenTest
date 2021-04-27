using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThrowAttack")]
public class ThrowAttack : Attack
{
    [SerializeField] GameObject projectile;
    [SerializeField] float force = 1f;

    public override void Action(Vector2 origin, Vector2 target, BeingType type)
    {
        if (!IsCooldownEnd())
            return;
        GameObject instantiated = Instantiate(projectile, origin, Quaternion.identity);
        instantiated.GetComponent<Projectile>().Init(target * force, type);
    }
}
