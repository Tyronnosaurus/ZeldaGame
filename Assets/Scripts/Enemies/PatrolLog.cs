using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolLog : Log
{
	/// <summary> Index of patrol point we currently want to reach </summary>
	public patrolPatterns patrolPattern;
	/// <summary> Patrol points </summary>
	public Transform[] patrolPoints;

	/// <summary> 
	/// Since patrolPoints are gameObjects inside this enemy, when the enemy moves they'll move as well (so we'd never reach any) 
	/// To solve that, we copy their positions at the start.
	/// </summary>
	public Vector3[] patrolPointsFixed;
	/// <summary> Index of patrol point we currently want to reach </summary>
	private int pIndex;
	/// <summary> Direction of patrol when going back and forth: 1 (forwards) or -1 (backwards) </summary>
	private int incr;

	/// <summary> Types of patrol available </summary>
	public enum patrolPatterns
	{
		backAndForth,
		loop
	}


	protected override void Start()
	{
		base.Start();
		
		ChangeState(EnemyState.walk);   // Patrol Log starts awake (never sleeps)
		anim.SetBool("wakeUp", true);

		// patrolPoints are gameobjects inside the enemy gameobject, thus they move along with it. We need to save and use their initial value only
		patrolPointsFixed = new Vector3[patrolPoints.Length];
		for (int i=0; i<patrolPoints.Length; i++) patrolPointsFixed[i] = patrolPoints[i].position;

		pIndex = 0; //We start going to point 0
	}


	private new void Update()
	{
		PatrolAndChase();
	}



	/// <summary>
	/// Make log follow a predefined patrol path and chase the player if he gets too close
	/// </summary>
	protected void PatrolAndChase()
	{
		float distanceToTarget = Vector3.Distance(target.position, transform.position);

		if (distanceToTarget < attackRadius)
		{
			// Do nothing
		}	
		else if (distanceToTarget <= chaseRadius)  // If player inside chase radius (but outside attack radius) -> Chase player
		{
			transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);  // Chase player
			changeAnimationOrientation(target.position - transform.position);       // Used to choose which walking animation to use
		}
		else if (distanceToTarget > chaseRadius)  // Outside chase radius -> Keep patrol
		{
			transform.position = Vector3.MoveTowards(transform.position, patrolPointsFixed[pIndex], moveSpeed * Time.deltaTime);  // Move to goal patrol point
			changeAnimationOrientation(patrolPointsFixed[pIndex] - transform.position);       // Used to choose which walking animation to use

			// When reaching a patrol point, go to the next
			if (Vector3.Distance(patrolPointsFixed[pIndex], transform.position) < 0.1)	ChangeToNextPatrolPoint();
		}

	}


	private void ChangeToNextPatrolPoint()
	{
		if (patrolPattern == patrolPatterns.backAndForth)
		{
			// Change direction at the ends (we follow a 'back and forth' pattern)
			if (pIndex == 0) incr = 1;  // Reached first point -> Change direction
			else if (pIndex == patrolPointsFixed.Length - 1) incr = -1;  // Reached last point  -> Change direction

			// Set index of next patrol point
			pIndex += incr;
		}
		else if (patrolPattern == patrolPatterns.loop)
		{
			if (pIndex == patrolPointsFixed.Length-1) pIndex = 0;
			else								      pIndex++;
		}
	}

}
