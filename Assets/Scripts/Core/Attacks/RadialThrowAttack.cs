using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RadialThrowAttack", menuName = "Attacks/RadialThrowAttack")]
public class RadialThrowAttack : Attack
{
    [SerializeField] Projectile projectile = default;
    [SerializeField] float force = 1f;
    [SerializeField] int projectileAmount = 1;

    public override void Action(Vector2 origin, Vector2 target, Being owner)
    {
        float spawnAngle = 360 / projectileAmount;

        for (int i = 0; i < projectileAmount; i++)
        {
            Vector3 direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (spawnAngle + spawnAngle * i)), Mathf.Cos(Mathf.Deg2Rad * (spawnAngle + spawnAngle * i)), 0);
            Instantiate(projectile, origin, Quaternion.identity).Init(direction * force, owner);
        }

    }
}
