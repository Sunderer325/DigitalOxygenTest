using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float maxY = 0;
    [SerializeField] float minY = 0;
    [HideInInspector] public GameObject target;

    private static CameraFollow _instance;
    public static CameraFollow Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<CameraFollow>();
            return _instance;
        }
    }
    private void LateUpdate()
    {
        if (target)
        {
            float desiredY = target.transform.position.y;
            if (target.transform.position.y < minY)
                desiredY = minY;
            transform.position = new Vector3(transform.position.x, desiredY, transform.position.z);
        }
    }
}
