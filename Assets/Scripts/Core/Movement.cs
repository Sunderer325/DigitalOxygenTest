using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Movement : MonoBehaviour
{
	public LayerMask collisionMask;
	public LayerMask headCollisionMask;
	[SerializeField] float skinWidth = .015f;
	[SerializeField] int horizontalRayCount = 4;
	[SerializeField] int verticalRayCount = 4;

	float horizontalRaySpacing;
	float verticalRaySpacing;
	public bool downThrough;

	new BoxCollider2D collider;
	public BoxCollider2D GetCollider => collider;
	RaycastOrigins raycastOrigins;
	public CollisionInfo collisions;

	public float GetSkinWidth => skinWidth;

	void Start()
	{
		collider = GetComponent<BoxCollider2D>();
		CalculateRaySpacing();
	}

	public void Move(Vector3 velocity)
	{
		if (collisionMask != 0 || headCollisionMask != 0)
		{
			UpdateRaycastOrigins();
			UpdateCollisions(ref velocity);
		}
		transform.Translate(velocity, Space.World);
	}

	public void UpdateCollisions(ref Vector3 velocity)
	{
		collisions.Reset();

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
		float rayLength = Mathf.Abs(velocity.x) + skinWidth;

		for (int i = 0; i < horizontalRayCount; i++)
		{
			Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
			rayOrigin += (Vector2)transform.up * (horizontalRaySpacing * i);
			RaycastHit2D hit;
			hit = Physics2D.Raycast(rayOrigin, (Vector2)transform.right * directionX, rayLength, headCollisionMask);

			Debug.DrawRay(rayOrigin, (Vector2)transform.right * directionX * rayLength, Color.red);

			if (hit)
			{
				velocity.x = (hit.distance - skinWidth) * directionX;
				rayLength = hit.distance;

				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
				collisions.any = true;
				collisions.target = hit.collider;
			}
		}
	}

	void VerticalCollisions(ref Vector3 velocity)
	{
		float directionY = Mathf.Sign(velocity.y);
		float rayLength = Mathf.Abs(velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i++)
		{
			Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
			rayOrigin += (Vector2)transform.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit;
			if (directionY > 0 || downThrough)
				hit = Physics2D.Raycast(rayOrigin, (Vector2)transform.up * directionY, rayLength, headCollisionMask);
			else
				hit = Physics2D.Raycast(rayOrigin, (Vector2)transform.up * directionY, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

			if (hit)
			{
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
				collisions.any = true;
				collisions.target = hit.collider;
			}
		}
	}

	void UpdateRaycastOrigins()
	{
		Bounds bounds = collider.bounds;
		bounds.Expand(skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
	}

	void CalculateRaySpacing()
	{
		Bounds bounds = collider.bounds;
		bounds.Expand(skinWidth * -2);

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
		public bool above, below;
		public bool left, right;
		public bool any;
		public Collider2D target;

		public void Reset()
		{
			above = below = false;
			left = right = false;
			any = false;
			target = null;
		}
	}

}