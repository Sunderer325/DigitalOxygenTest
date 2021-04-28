using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpearAttack")]
public class SpearAttack : Attack
{
    public override GameObject Action(Vector2 origin, Vector2 target, BeingType type)
    {
        IsCooldownEnd();
        Debug.Log("SPEAR ATTACK");
        return null;
    }
}
