using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Being
{
	[SerializeField] public Attack[] attacks;
	[SerializeField] int delayToJump = 2;

	int frameCounter = 0;
	bool jump, reminder;

	protected override void Start()
	{
		base.Start();
		beingType = BeingType.PLAYER;
		UIManager.Instance.SetHP(health);
	}

	protected override void CollisionsUpdate()
	{
		if (movement.collisions.above && velocity.y > 0)
			velocity.y = 0;
		else if (movement.collisions.below)
		{
			inAir = false;
			jump = false;
			reminder = false; velocity.y = 0;
		}
		else 
		{
			inAir = true;
			if (!jump && !reminder)
			{
				frameCounter = Time.frameCount;
				reminder = true;
			}
		}

		Jump();
	}

	private void Jump()
	{
		if (PlayerInput.Instance.GetPlayerInput.y == -2)
			movement.downThrough = false;
		else if(PlayerInput.Instance.GetPlayerInput.y == -1)
			movement.downThrough = true;

		if (inAir)
		{
			if (PlayerInput.Instance.GetPlayerInput.y == 2 && velocity.y > minJumpVelocity)
				velocity.y = minJumpVelocity;

			if (Time.frameCount - frameCounter > delayToJump)
				return;
		}

		if(PlayerInput.Instance.GetPlayerInput.y == 1)
		{
			velocity.y = maxJumpVelocity;   //jump button is down
			jump = true;
		}
	}

	protected override void ForcedMovement()
	{
		forcedMovementVelocity.x += PlayerInput.Instance.GetPlayerInput.x * forcedMovementSpeed;
		forcedMovementVelocity.y += gravity * Time.deltaTime;
		movement.Move(forcedMovementVelocity * Time.deltaTime);
	}

	protected override void NormalMovement()
	{
		velocity.x = PlayerInput.Instance.GetPlayerInput.x * moveSpeed;
		velocity.y += gravity * Time.deltaTime;
		movement.Move(velocity * Time.deltaTime);
	}
}
