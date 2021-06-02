using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RadialThrowAttack", menuName = "Attacks/RadialThrowAttack")]
public class RadialThrowAttack : Attack
{
    [SerializeField] GameObject projectile;
    [SerializeField] float force = 1f;
    [SerializeField] int projectileAmount = 1;

    public override void Action(Vector2 origin, Vector2 target, Being owner)
    {
        if (!IsCooldownEnd())
            return;

        float spawnAngle = 360 / projectileAmount;

        for (int i = 0; i < projectileAmount; i++)
        {
            GameObject instantiated = Instantiate(projectile, origin, Quaternion.identity);
            instantiated.GetComponent<Projectile>().Init(new Vector3(Mathf.Sin(Mathf.Deg2Rad * (spawnAngle + spawnAngle * i)), Mathf.Cos(Mathf.Deg2Rad * (spawnAngle + spawnAngle * i)), 0), owner);
        }

    }
}
