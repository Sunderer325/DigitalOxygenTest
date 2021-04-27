using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
	int moveDir;
	Vector2 attackPosition;

	protected override void Start()
    {
		base.Start();
		type = EnemyType.GROUND;
		moveDir = Random.value > 0.5f ? 1 : -1;
    }

	protected override void Update()
	{
		base.Update();
		if (isDie) return;

		attackPosition = (Vector2)transform.position + movement.GetCollider.offset;
		attack.Action(attackPosition, Vector2.zero, beingType);

		if (movement.collisions.below)
			velocity.y = 0;

		if (movement.collisions.left || movement.collisions.right)
			moveDir *= -1;


		velocity.x = moveDir * moveSpeed;
		velocity.y += gravity * Time.deltaTime;
		movement.Move(velocity * Time.deltaTime);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(attackPosition, 0.5f);
	}

}
