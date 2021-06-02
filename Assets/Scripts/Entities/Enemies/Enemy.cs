using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Being
{
	[SerializeField] protected Attack attack;
	protected GameObject target;
	protected Collider2D targetCollider;
	protected EnemyType type;
	public new EnemyType GetType => type;

	protected override void Start()
	{
		base.Start();
		beingType = BeingType.ENEMY;
		target = GameObject.FindGameObjectWithTag("Player");
		targetCollider = target.GetComponent<Collider2D>();
	}

	protected override void Update()
	{
		base.Update();
		if (isDie) return;

		AttackTarget();
	}

	protected virtual bool AttackTarget()
	{
		Vector2 attackPosition = (Vector2)transform.position + movement.GetCollider.offset;
		Vector2 contactPoint = targetCollider.ClosestPoint(attackPosition);
		if (Vector2.Distance(attackPosition, contactPoint) <= (attack as MeleeAttack).DamageRadius)
		{
 			attack.Action(attackPosition, target.transform.position, this);
			return true;
		}
		return false;
	}

	protected override void Die()
	{
		base.Die();
		EntityManager.Instance.OnEnemyDie();
	}

	//private void OnDrawGizmos()
	//{
	//	Gizmos.color = Color.red;
	//	Gizmos.DrawWireSphere((Vector2)transform.position + movement.GetCollider.offset, attack.DamageRadius);
	//	Gizmos.color = Color.green;
	//	Gizmos.DrawWireCube((Vector2)transform.position + movement.GetCollider.offset, movement.GetCollider.size);
	//}
}

public enum EnemyType
{
	GROUND,
	AIR
}