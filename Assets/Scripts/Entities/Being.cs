using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Animation))]
public abstract class Being : MonoBehaviour
{
	[SerializeField] protected int health;
	[SerializeField] protected float moveSpeed = 6f;
	[SerializeField] protected float forcedMovementSpeed = 2f;
	[SerializeField] protected float gravity = -0.1f;

	float afterDeathTimer = 10f;
	float afterDeathFriction = 0.5f;
	protected bool forcedMovement;
	protected Vector3 forcedMovementVelocity;

	protected Vector2 velocity;
	public Vector2 Velocity => velocity; 

	protected bool inAir;
	public bool InAir => inAir; 

	protected bool isDie;
	public bool IsDie => isDie;
	protected BeingType beingType;
	public BeingType GetBeingType => beingType;

	protected Movement movement;
	Animation animation;

	protected virtual void Start()
	{
		movement = GetComponent<Movement>();
		animation = GetComponent<Animation>();
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
	}

	void AfterDeathUpdate()
	{
		if (movement.collisions.below)
		{
			velocity.x *= afterDeathFriction;
			inAir = false;
		}
		velocity.y += gravity * Time.deltaTime;
		movement.Move(velocity * Time.deltaTime);
	}

	public virtual void GetDamage(int damage, Vector2 force)
	{
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

	}
}

public enum BeingType
{
	PLAYER,
	ENEMY 
}
