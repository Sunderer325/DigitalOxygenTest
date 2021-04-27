using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Being
{
	[SerializeField] public Attack[] attacks;
	[SerializeField] float jumpHeight = 4;
	[SerializeField] float timeToJumpApex = 0.4f;
	[SerializeField] float invulnerabilityTime = 0.2f;

	float jumpVelocity;
	float invulnerabilityTimer;

	protected override void Start()
	{
		base.Start();
		beingType = BeingType.PLAYER;
		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
	}

	protected override void Update()
	{
		base.Update();
		if (isDie) return;

		if (movement.collisions.above && velocity.y > 0)
			velocity.y = 0;
		else if (movement.collisions.below)
		{
			if (PlayerInput.Instance.GetPlayerInput.y > 0.4f)
			{
				velocity.y = jumpVelocity;
				inAir = true;
			}
			else
			{
				velocity.y = 0;
				inAir = false;
			}
		}
		else inAir = true;

		if (forcedMovement)
		{
			forcedMovementVelocity.x += PlayerInput.Instance.GetPlayerInput.x * forcedMovementSpeed;
			forcedMovementVelocity.y += gravity * Time.deltaTime;
			movement.Move(forcedMovementVelocity * Time.deltaTime);
		}
		else
		{
			velocity.x = PlayerInput.Instance.GetPlayerInput.x * moveSpeed;
			velocity.y += gravity * Time.deltaTime;
			movement.Move(velocity * Time.deltaTime);
		}
	}
}
