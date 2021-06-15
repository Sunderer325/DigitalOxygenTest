using UnityEngine;

[RequireComponent(typeof(AudioPrefab))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(BeingAnimation))]
public abstract class Being : MonoBehaviour
{
	#region Physics properties
	[Header("Physics")]
	[SerializeField] protected float moveSpeed = 6f;
	[SerializeField] protected float moveSpeedOnStunning = 1f;

	[SerializeField] protected float forcedMovementSpeed = 2f;
	[SerializeField] protected float forcedMovementTime = 0.3f;

	[SerializeField] protected float maxJumpHeight = 4;
	[SerializeField] protected float minJumpHeight = 1;
	[SerializeField] protected float timeToJumpApex = 0.4f;

	protected Movement movement;
	public Vector2 Velocity;

	protected bool forcedMovement;
	public Vector2 ForcedMovementVelocity;
	protected float forcedMovementTimer;

	protected float gravity = -0.1f;
	protected float maxJumpVelocity;
	protected float minJumpVelocity;
	#endregion

	#region Stats properties
	[Header("Stats")]
	[SerializeField] protected int health;
	[SerializeField] float invulnerabilityTime = 0.1f;
	
	public BeingType BeingType { get; protected set; }

	public bool Invulnerability { get; private set; }
	float invulnerabilityTimer;

	public bool InAir { get; protected set; }
	public bool IsDie { get; protected set; }

	float afterDeathTimer = 10f;
	float afterDeathFriction = 0.5f;
	
	protected bool stunning;
	float stunTime;
	float remindSpeed;
	#endregion

	protected new BeingAnimation animation;
	protected new AudioPrefab audio;

	protected virtual void Start()
	{
		movement = GetComponent<Movement>();
		animation = GetComponent<BeingAnimation>();
		audio = GetComponent<AudioPrefab>();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
	}

	protected virtual void Update()
	{
		if (GameManager.Instance.GameState != GameStates.GAME &&
			GameManager.Instance.GameState != GameStates.WIN)
		{
			audio.Stop();
			return;
		}

		CollisionsUpdate();

		if (IsDie)
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
		if (movement.Collisions.Below)
		{
			ForcedMovementVelocity.x *= afterDeathFriction;
			InAir = false;
		}
		ForcedMovementVelocity.y += gravity * Time.deltaTime;
		movement.Move(ForcedMovementVelocity * Time.deltaTime);
	}

	void InvulnerabilityUpdate()
	{
		if (Invulnerability)
		{
			if (invulnerabilityTimer >= invulnerabilityTime)
			{
				Invulnerability = false;
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
			if (movement.Collisions.Any)
			{
				forcedMovementTimer = 0f;
				forcedMovement = false;
				ForcedMovementVelocity = Vector2.zero;
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
		if (Invulnerability)
			return;
		Invulnerability = true;

		health -= damage;

		animation.GetHit();

		if(force != Vector2.zero)
		{
			forcedMovement = true;
			ForcedMovementVelocity = force;
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
		IsDie = true;
		Velocity = ForcedMovementVelocity;
		gameObject.layer = LayerMask.NameToLayer("Corpse");
		gameObject.tag = "Untagged";
		GetComponent<SpriteRenderer>().sortingLayerName = "Corpse";

		audio.Stop();
	}
}

public enum BeingType
{
	PLAYER,
	ENEMY 
}
