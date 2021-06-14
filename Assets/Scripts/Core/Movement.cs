using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Movement : MonoBehaviour
{
	public LayerMask CollisionMask;
	public LayerMask HeadCollisionMask;
	public float SkinWidth = .015f;
	[SerializeField] int horizontalRayCount = 4;
	[SerializeField] int verticalRayCount = 4;

	float horizontalRaySpacing;
	float verticalRaySpacing;
	[HideInInspector] public bool DownThrough;

	[HideInInspector] public BoxCollider2D Collider { get; private set; }
	RaycastOrigins raycastOrigins;
	public CollisionInfo Collisions;

	void Start()
	{
		Collider = GetComponent<BoxCollider2D>();
		CalculateRaySpacing();
	}

	public void Move(Vector3 velocity)
	{
		if (CollisionMask != 0 || HeadCollisionMask != 0)
		{
			UpdateRaycastOrigins();
			UpdateCollisions(ref velocity);
		}
		transform.Translate(velocity, Space.World);
	}

	public void UpdateCollisions(ref Vector3 velocity)
	{
		Collisions.Reset();

		if (velocity.x != 0)
		{
			HorizontalCollisions(ref velocity);
		}
		if (velocity.y != 0)
		{
			VerticalCollisions(ref velocity);
		}
	}

	void HorizontalCollisions(ref Vector3 velocity)
	{
		float directionX = Mathf.Sign(velocity.x);
		float rayLength = Mathf.Abs(velocity.x) + SkinWidth;

		for (int i = 0; i < horizontalRayCount; i++)
		{
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += (Vector2)transform.up * (horizontalRaySpacing * i);
			RaycastHit2D hit;
			hit = Physics2D.Raycast(rayOrigin, (Vector2)transform.right * directionX, rayLength, HeadCollisionMask);
			
			Debug.DrawRay(rayOrigin, (Vector2)transform.right * directionX * rayLength, Color.red);

			if (hit)
			{
				velocity.x = (hit.distance - SkinWidth) * directionX;
				rayLength = hit.distance;

				Collisions.Left = directionX == -1;
				Collisions.Right = directionX == 1;
				Collisions.Any = true;
				Collisions.Target = hit.collider;
				Collisions.Normal = hit.normal;
			}
		}
	}

	void VerticalCollisions(ref Vector3 velocity)
	{
		float directionY = Mathf.Sign(velocity.y);
		float rayLength = Mathf.Abs(velocity.y) + SkinWidth;

		for (int i = 0; i < verticalRayCount; i++)
		{
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += (Vector2)transform.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit;
			if (directionY > 0 || DownThrough)
				hit = Physics2D.Raycast(rayOrigin, (Vector2)transform.up * directionY, rayLength, HeadCollisionMask);
			else
				hit = Physics2D.Raycast(rayOrigin, (Vector2)transform.up * directionY, rayLength, CollisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

			if (hit)
			{
				velocity.y = (hit.distance - SkinWidth) * directionY;
				rayLength = hit.distance;

				Collisions.Below = directionY == -1;
				Collisions.Above = directionY == 1;
				Collisions.Any = true;
				Collisions.Target = hit.collider;
				Collisions.Normal = hit.normal;
			}
		}
	}

	void UpdateRaycastOrigins()
	{
		Bounds bounds = Collider.bounds;
		bounds.Expand(SkinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	void CalculateRaySpacing()
	{
		Bounds bounds = Collider.bounds;
		bounds.Expand(SkinWidth * -2);

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	struct RaycastOrigins
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	public struct CollisionInfo
	{
		public bool Above, Below;
		public bool Left, Right;
		public bool Any;
		public Collider2D Target;
		public Vector2 Normal;

		public void Reset()
		{
			Above = Below = false;
			Left = Right = false;
			Any = false;
			Target = null;
		}
	}

}