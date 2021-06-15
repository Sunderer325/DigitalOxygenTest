using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : ScriptableObject
{
    public float CoolDown = 0;

    protected float lastTime = 0;

    private void OnEnable()
    {
        lastTime = 0;
    }

    public bool IsCooldownEnd()
    {
        if (lastTime != 0 && CoolDown != 0)
        {
            if (Time.unscaledTime - lastTime > CoolDown)
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
    public abstract void Action(Vector2 origin, Vector2 target, Being owner);
}
