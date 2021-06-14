using UnityEngine;

public abstract class Enemy : Being
{
	[SerializeField] protected Attack attack;
	protected GameObject target;
	protected Collider2D targetCollider;
	public EnemyType EnemyType { get; protected set; }

	protected override void Start()
	{
		base.Start();
		BeingType = BeingType.ENEMY;
		target = GameObject.FindGameObjectWithTag("Player");
		targetCollider = target.GetComponent<Collider2D>();
	}

	protected override void Update()
	{
		base.Update();
		if (IsDie) return;

		AttackTarget();
	}

	protected virtual bool AttackTarget()
	{
		if (attack.IsCooldownEnd() && !stunning)
		{
			Vector2 attackPosition = (Vector2)transform.position + movement.Collider.offset;
			Vector2 contactPoint = targetCollider.ClosestPoint(attackPosition);
			if (Vector2.Distance(attackPosition, contactPoint) <= (attack as MeleeAttack).DamageRadius)
			{
				attack.Action(attackPosition, target.transform.position, this);
				Debug.Log(gameObject.name + " attacks " + target.name + " with " + attack.name);
				return true;
			}
		}
		return false;
	}

	public override void GetDamage(int damage, Vector2 force)
	{
		if (Invulnerability)
			return;
		base.GetDamage(damage, force);

		audio.Play("enemy_hit");
	}

	protected override void Die()
	{
		base.Die();
		EntityManager.Instance.OnEnemyDie();
	}
}

public enum EnemyType
{
	GROUND,
	AIR
}