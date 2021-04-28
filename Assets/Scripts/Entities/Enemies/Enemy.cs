using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Being
{
	[SerializeField] protected Attack attack;
	protected GameObject target;
	protected EnemyType type;
	public new EnemyType GetType => type;

	protected override void Start()
	{
		base.Start();
		beingType = BeingType.ENEMY;
		target = GameObject.FindGameObjectWithTag("Player");
	}
}

public enum EnemyType
{
	GROUND,
	AIR
}