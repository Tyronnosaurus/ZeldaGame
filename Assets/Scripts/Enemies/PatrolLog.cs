using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolLog : Log
{
	/// <summary> Index of patrol point we currently want to reach </summary>
	public Transform[] patrolPoints;
	/// <summary> Index of patrol point we currently want to reach </summary>
	public patrolPatterns patrolPattern;

	/// <summary> Index of patrol point we currently want to reach </summary>
	private int pIndex;
	/// <summary> Direction of patrol: 1 (forwards) or -1 (backwards) </summary>
	private int incr;

	/// <summary> Types of patrol available </summary>
	public enum patrolPatterns
	{
		backAndForth,
		loop
	}


	private new void Start()
	{
		base.Start();
		
		ChangeState(EnemyState.walk);   // Patrol Log starts awake (never sleeps)
		anim.SetBool("wakeUp", true);

		pIndex = 0;
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

		if (attackRadius < distanceToTarget)
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
			transform.position = Vector3.MoveTowards(transform.position, patrolPoints[pIndex].position, moveSpeed * Time.deltaTime);  // Move to goal patrol point
			changeAnimationOrientation(patrolPoints[pIndex].position - transform.position);       // Used to choose which walking animation to use

			// When reaching a patrol point, go to the next
			if (Vector3.Distance(patrolPoints[pIndex].position, transform.position) < 0.1)	ChangeToNextPatrolPoint();
		}

	}


	private void ChangeToNextPatrolPoint()
	{
		if (patrolPattern == patrolPatterns.backAndForth)
		{
			// Change direction at the ends (we follow a 'back and forth' pattern)
			if (pIndex == 0) incr = 1;  // Reached first point -> Change direction
			else if (pIndex == patrolPoints.Length - 1) incr = -1;  // Reached last point  -> Change direction

			// Set index of next patrol point
			pIndex += incr;
		}
		else if (patrolPattern == patrolPatterns.loop)
		{
			if (pIndex == patrolPoints.Length - 1) pIndex = 0;
			else								   pIndex++;
		}
	}

}
