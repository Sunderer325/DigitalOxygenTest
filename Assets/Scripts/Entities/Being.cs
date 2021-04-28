using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Animation))]
public abstract class Being : MonoBehaviour
{
	[Header("Stats")]
	[SerializeField] protected int health;
	[SerializeField] float invulnerabilityTime = 0.1f;

	float afterDeathTimer = 10f;
	float afterDeathFriction = 0.5f;
	float invulnerabilityTimer;
	bool invulnerability;

	[Header("Physics")]
	[SerializeField] protected float moveSpeed = 6f;
	[SerializeField] protected float forcedMovementSpeed = 2f;
	[SerializeField] protected float maxJumpHeight = 4;
	[SerializeField] protected float minJumpHeight = 1;
	[SerializeField] protected float timeToJumpApex = 0.4f;

	protected float gravity = -0.1f;
	protected float maxJumpVelocity;
	protected float minJumpVelocity;

	protected Movement movement;
	Animation animation;

	protected bool forcedMovement;
	protected Vector2 forcedMovementVelocity;
	protected Vector2 velocity;
	protected bool inAir;
	protected bool isDie;
	protected BeingType beingType;

	public bool Invulnerability => invulnerability;
	public Vector2 ForcedMovementVelocity => forcedMovementVelocity;
	public Vector2 Velocity => velocity; 
	public bool InAir => inAir; 
	public bool IsDie => isDie;
	public BeingType GetBeingType => beingType;

	protected virtual void Start()
	{
		movement = GetComponent<Movement>();
		animation = GetComponent<Animation>();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
	}

	protected virtual void Update()
	{
		if (isDie && afterDeathTimer > 0) { 
			AfterDeathUpdate();
			afterDeathTimer -= Time.deltaTime;
		}
		if (forcedMovement && movement.collisions.any)
		{
			forcedMovement = false;
			forcedMovementVelocity = Vector2.zero;
		}
		if (invulnerability)
		{
			if(invulnerabilityTimer >= invulnerabilityTime)
			{
				invulnerability = false;
				invulnerabilityTimer = 0f;
			}
			else
			{
				invulnerabilityTimer += Time.deltaTime;
			}
		}
		CollisionsUpdate();

		if (isDie) return;
		if (forcedMovement)
			ForcedMovement();
		else NormalMovement();
	}
	protected abstract void NormalMovement();
	protected abstract void ForcedMovement();
	protected abstract void CollisionsUpdate();

	void AfterDeathUpdate()
	{
		if (movement.collisions.below)
		{
			velocity.x *= afterDeathFriction;
			inAir = false;
		}
		forcedMovementVelocity.y += gravity * Time.deltaTime;
		movement.Move(forcedMovementVelocity * Time.deltaTime);
	}

	public virtual void GetDamage(int damage, Vector2 force)
	{
		if (invulnerability)
			return;
		invulnerability = true;

		health -= damage;
		animation.GetHit();

		if (health <= 0)
		{
			isDie = true;
			velocity = Vector2.zero;
			gameObject.layer = LayerMask.NameToLayer("Corpse");

			if (beingType == BeingType.ENEMY)
				EntityManager.Instance.OnEnemyDie();
			else if (beingType == BeingType.PLAYER)
				EntityManager.Instance.OnPlayerDie();
		}

		if(force != Vector2.zero)
		{
			forcedMovement = true;
			forcedMovementVelocity = force;
			movement.Move(forcedMovementVelocity * Time.deltaTime);
		}
		if (beingType == BeingType.PLAYER)
			UIManager.Instance.SetHP(health);
	}
}

public enum BeingType
{
	PLAYER,
	ENEMY 
}
