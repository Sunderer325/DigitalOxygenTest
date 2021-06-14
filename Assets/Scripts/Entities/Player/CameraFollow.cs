using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float minY = 0;
    [SerializeField] Vector2 offset = Vector2.zero;
    [HideInInspector] public GameObject Target;
    [HideInInspector] public Camera Camera;

    private static CameraFollow _instance;
    public static CameraFollow Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraFollow>();
            }
            return _instance;
        }
    }

    private void Start()
    {
        Camera = GetComponent<Camera>();
    }
    private void LateUpdate()
    {
        if (Target)
        {
            float desiredY = Target.transform.position.y;
            if (Target.transform.position.y < minY)
                desiredY = minY;
            transform.position = new Vector3(transform.position.x, desiredY, transform.position.z) + (Vector3)offset;
        }
    }
}
