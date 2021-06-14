using System;
using UnityEngine;

public class GroundEnemy : Enemy
{
	[SerializeField, Range(0f, 1f)] float jumpChance = 0.5f;
	int moveDir;
	Vector2 attackPosition;

	protected override void Start()
    {
		base.Start();
		EnemyType = EnemyType.GROUND;
		moveDir = UnityEngine.Random.value > 0.5f ? 1 : -1;
	}

	protected override void Update()
	{
		base.Update();
		if (IsDie) return;

		Vector2 delta = target.transform.position - transform.position;
		if (Math.Abs(delta.x) < 2 && delta.y < maxJumpHeight && delta.y > 1)
		{
			if(!forcedMovement && stunning)
				Jump();
		}
	}

	void Jump()
	{
		if (UnityEngine.Random.value > jumpChance * Time.fixedDeltaTime)
			return;

		TurnToTargetDirection();
		float jumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * (target.transform.position.y - transform.position.y + 1));
		forcedMovement = true;
		ForcedMovementVelocity = new Vector2(moveDir * moveSpeed, jumpVelocity);
		movement.Move(ForcedMovementVelocity * Time.deltaTime);
	}

	void TurnToTargetDirection()
	{
		moveDir = target.transform.position.x > transform.position.x ? 1 : -1;
	}

	protected override void CollisionsUpdate()
	{
		if (movement.Collisions.Below)
			Velocity.y = 0;

		if (movement.Collisions.Left || movement.Collisions.Right)
			moveDir *= -1;
	}
	protected override void NormalMovement()
	{
		Velocity.x = moveDir * moveSpeed;
		if(Mathf.Abs(Velocity.y) < 10f)
			Velocity.y += gravity * Time.deltaTime;
		movement.Move(Velocity * Time.deltaTime);
	}
	protected override void ForcedMovement()
	{
		ForcedMovementVelocity.y += gravity * Time.deltaTime;
		movement.Move(ForcedMovementVelocity * Time.deltaTime);
	}
}
