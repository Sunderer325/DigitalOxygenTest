using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Being
{
	[SerializeField] protected Attack attack;
	protected EnemyType type;
	public new EnemyType GetType => type;

	protected override void Start()
	{
		base.Start();
		beingType = BeingType.ENEMY;
	}
}

public enum EnemyType
{
	GROUND,
	AIR
}