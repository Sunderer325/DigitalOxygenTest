using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(BeingAnimation))]
public abstract class Being : MonoBehaviour
{
	#region Physics properties
	[Header("Physics")]
	[SerializeField] protected float moveSpeed = 6f;
	[SerializeField] protected float moveSpeedOnStunning = 1f;
	[SerializeField] protected float forcedMovementSpeed = 2f;
	[SerializeField] protected float maxJumpHeight = 4;
	[SerializeField] protected float minJumpHeight = 1;
	[SerializeField] protected float timeToJumpApex = 0.4f;

	protected float gravity = -0.1f;
	protected float maxJumpVelocity;
	protected float minJumpVelocity;
	#endregion

	#region Stats properties
	[Header("Stats")]
	[SerializeField] protected int health;
	[SerializeField] float invulnerabilityTime = 0.1f;

	float afterDeathTimer = 10f;
	float afterDeathFriction = 0.5f;
	float invulnerabilityTimer;
	[HideInInspector] public bool invulnerability;
	[SerializeField] protected float forcedMovementTime = 0.3f;
	
	protected bool stunning;
	float stunTime;
	float remindSpeed;
	#endregion

	#region Other protected properties
	protected Movement movement;
	protected BeingAnimation animation;
	protected bool forcedMovement;
	protected Vector2 forcedMovementVelocity;
	protected float forcedMovementTimer;
	protected Vector2 velocity;
	protected bool inAir;
	protected bool isDie;
	protected BeingType beingType;
	#endregion

	#region Public properties
	public bool Invulnerability => invulnerability;
	public Vector2 ForcedMovementVelocity => forcedMovementVelocity;
	public Vector2 Velocity => velocity; 
	public bool InAir => inAir; 
	public bool IsDie => isDie;
	public BeingType GetBeingType => beingType;
	#endregion

	protected virtual void Start()
	{
		movement = GetComponent<Movement>();
		animation = GetComponent<BeingAnimation>();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
	}

	protected virtual void Update()
	{
		if (GameManager.Instance.GetState != GameStates.GAME)
			return;

		CollisionsUpdate();

		if (isDie)
		{
			AfterDeathUpdate();
			return;
		}
		InvulnerabilityUpdate();
		if (forcedMovement)
		{
			ForcedMovement();
			ForcedUpdate();
		}
		else NormalMovement();

		if (stunning)
		{
			if(stunTime <= 0f)
			{
				stunning = false;
				moveSpeed = remindSpeed;
				return;
			}
			stunTime -= Time.deltaTime;
		}
	}
	protected abstract void CollisionsUpdate();
	protected abstract void NormalMovement();
	protected abstract void ForcedMovement();
	void AfterDeathMovement()
	{
		if (movement.collisions.below)
		{
			velocity.x *= afterDeathFriction;
			inAir = false;
		}
		velocity.y += gravity * Time.deltaTime;
		movement.Move(velocity * Time.deltaTime);
	}

	void InvulnerabilityUpdate()
	{
		if (invulnerability)
		{
			if (invulnerabilityTimer >= invulnerabilityTime)
			{
				invulnerability = false;
				invulnerabilityTimer = 0f;
			}
			else
			{
				invulnerabilityTimer += Time.deltaTime;
			}
		}
	}
	void ForcedUpdate()
	{
		if (forcedMovementTimer > forcedMovementTime)
		{
			if (movement.collisions.any)
			{
				forcedMovementTimer = 0f;
				forcedMovement = false;
				forcedMovementVelocity = Vector2.zero;
			}
		}
		else
		{
			forcedMovementTimer += Time.deltaTime;
		}
	}
	void AfterDeathUpdate()
	{
		if (afterDeathTimer > 0)
		{
			afterDeathTimer -= Time.deltaTime;
		}
		AfterDeathMovement();
	}

	public virtual void GetDamage(int damage, Vector2 force)
	{
		if (invulnerability)
			return;
		invulnerability = true;

		health -= damage;

		animation.GetHit();

		if(force != Vector2.zero)
		{
			forcedMovement = true;
			forcedMovementVelocity = force;
			//movement.Move((forcedMovementVelocity - velocity) * Time.deltaTime);
		}

		if (health <= 0)
		{
			Die();
		}
	}
	public void Stunning(float onTime)
	{
		stunning = true;
		stunTime = onTime;
		remindSpeed = moveSpeed;
		moveSpeed = moveSpeedOnStunning;
	}

	protected virtual void Die()
	{
		isDie = true;
		velocity = forcedMovementVelocity;
		gameObject.layer = LayerMask.NameToLayer("Corpse");
		gameObject.tag = "Untagged";
		movement.collisionMask = LayerMask.GetMask("Obstacle", "Platform");
		movement.headCollisionMask = LayerMask.GetMask("Obstacle", "Platform");
		GetComponent<SpriteRenderer>().sortingLayerName = "Corpse";
	}
}

public enum BeingType
{
	PLAYER,
	ENEMY 
}
