using UnityEngine;

public class Player : Being
{
	[SerializeField] public Attack[] attacks;
	[SerializeField] int delayToJump = 2;

	[Header("Events")]
	[SerializeField] private IntEvent OnHealthChanged = default;

	int frameCounter = 0;
	bool jump, reminder;

	protected override void Start()
	{
		base.Start();
		BeingType = BeingType.PLAYER;
	}

	protected override void Update()
	{
		base.Update();

		if (GameManager.Instance.PlayerCheck)
		{
			if (movement.Collisions.Any)
				GameManager.Instance.PlayerCheck = false;
		}
	}

	protected override void CollisionsUpdate()
	{
		if (movement.Collisions.Above && Velocity.y > 0)
			Velocity.y = 0;
		else if (movement.Collisions.Below)
		{
			InAir = false;
			jump = false;
			reminder = false; Velocity.y = 0;
		}
		else 
		{
			InAir = true;
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
			movement.DownThrough = false;
		else if(PlayerInput.Instance.GetPlayerInput.y == -1)
			movement.DownThrough = true;

		if (InAir)
		{
			if (PlayerInput.Instance.GetPlayerInput.y == 2 && Velocity.y > minJumpVelocity)
				Velocity.y = minJumpVelocity;

			if (Time.frameCount - frameCounter > delayToJump)
				return;
		}

		if(PlayerInput.Instance.GetPlayerInput.y == 1)
		{
			Velocity.y = maxJumpVelocity;   //jump button is down
			jump = true;

			audio.Play("hero_jump", 0.5f);
		}
	}

	protected override void ForcedMovement()
	{
		ForcedMovementVelocity.x += PlayerInput.Instance.GetPlayerInput.x * forcedMovementSpeed;
		ForcedMovementVelocity.y += gravity * Time.deltaTime;
		movement.Move(ForcedMovementVelocity * Time.deltaTime);
	}

	protected override void NormalMovement()
	{
		Velocity.x = PlayerInput.Instance.GetPlayerInput.x * moveSpeed;
		Velocity.y += gravity * Time.deltaTime;
		movement.Move(Velocity * Time.deltaTime);
	}

	public override void GetDamage(int damage, Vector2 force)
	{
		if (Invulnerability)
			return;
		base.GetDamage(damage, force);
		OnHealthChanged.Raise(health);

		audio.Play("hero_hit");
	}

	protected override void Die()
	{
		IsDie = true;
		GameManager.Instance.GameState = GameStates.LOSE;
		audio.Stop();
	}

	public void DisableMovement()
	{
		Velocity = Vector2.zero;
		movement.enabled = false;
		PlayerInput.Instance.enabled = false;
		gameObject.layer = 0;
	}
}
