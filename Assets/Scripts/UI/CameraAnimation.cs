using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Animator))]
public class CameraAnimation : MonoBehaviour
{
    Animator animator;
    Camera camera;
    void Start()
    {
        animator = GetComponent<Animator>();
        camera = GetComponent<Camera>();
    }

    public void OnEnemyKilled()
    {
        //animator.SetTrigger("OnEnemyKilled");
    }

    public void OnPlayerHit()
    {
        animator.SetTrigger("OnPlayerHit");
    }
}
