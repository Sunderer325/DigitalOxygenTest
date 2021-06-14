using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Lift : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Movement movement;
    public bool isPlayerOnIt;
    public BoxCollider2D trigger;
}
