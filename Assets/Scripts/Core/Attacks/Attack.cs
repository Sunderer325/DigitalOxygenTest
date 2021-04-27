using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : ScriptableObject
{
    [SerializeField] float coolDown = 0;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float knockbackForce = 5f;
    protected float lastTime = 0;

    private void OnEnable()
    {
        lastTime = 0;
    }

    protected bool IsCooldownEnd()
    {
        if (lastTime != 0 && coolDown != 0)
        {
            if (Time.unscaledTime - lastTime > coolDown)
            {
                lastTime = Time.unscaledTime;
                return true;
            }
        }
        else
        {
            lastTime = Time.unscaledTime;
            return true;
        }
        return false;
    }
    public abstract void Action(Vector2 origin, Vector2 target, BeingType beingType);
}
