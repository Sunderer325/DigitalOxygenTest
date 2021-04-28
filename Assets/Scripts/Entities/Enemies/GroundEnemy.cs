using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
	[SerializeField, Range(0f, 1f)] float jumpChance = 0.5f;
	int moveDir;
	Vector2 attackPosition;

	protected override void Start()
    {
		base.Start();
		type = EnemyType.GROUND;
		moveDir = UnityEngine.Random.value > 0.5f ? 1 : -1;
	}

	protected override void Update()
	{
		base.Update();
		if (isDie) return;

		attackPosition = (Vector2)transform.position + movement.GetCollider.offset;
		attack.Action(attackPosition, Vector2.zero, beingType);

		float deltaY = target.transform.position.y - transform.position.y;
		if (deltaY < maxJumpHeight && !forcedMovement)
		{
			Jump();
		}
		if (target.transform.position.y < transform.position.y + 0.2f
			&& target.transform.position.y > transform.position.y - 0.2f)
			TurnToTargetDirection();
	}

	void Jump()
	{
		if (UnityEngine.Random.value > jumpChance)
			return;

		TurnToTargetDirection();
		float jumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * (target.transform.position.y - transform.position.y + 1));
		forcedMovement = true;
		forcedMovementVelocity = new Vector2(moveDir * moveSpeed, jumpVelocity);
		movement.Move(forcedMovementVelocity * Time.deltaTime);
	}

	void TurnToTargetDirection()
	{
		moveDir = target.transform.position.x > transform.position.x ? 1 : -1;
	}

	protected override void CollisionsUpdate()
	{
		if (movement.collisions.below)
			velocity.y = 0;

		if (movement.collisions.left || movement.collisions.right)
			moveDir *= -1;
	}
	protected override void NormalMovement()
	{
		velocity.x = moveDir * moveSpeed;
		if(Mathf.Abs(velocity.y) < 20f)
			velocity.y += gravity * Time.deltaTime;
		movement.Move(velocity * Time.deltaTime);
	}
	protected override void ForcedMovement()
	{
		forcedMovementVelocity.y += gravity * Time.deltaTime;
		movement.Move(forcedMovementVelocity * Time.deltaTime);
	}
}
