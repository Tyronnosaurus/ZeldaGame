using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEnemy : Log
{
	[Header("Boundary")]
	public Collider2D boundary;



    // Just like the base function but chases only if player is within a cerain area
    protected override void CheckDistance()
    {
		float distance = Vector3.Distance(target.position, transform.position);

		if (distance <= chaseRadius  && boundary.bounds.Contains(target.transform.position))
		{
			if (currentState == EnemyState.idle)
			{
				ChangeState(EnemyState.walk);  // Player gets close -> Awake
				anim.SetBool("wakeUp", true);
			}

			if ((distance > attackRadius) && (currentState == EnemyState.walk))
			{
				transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);  // If awake, chase player
				changeAnimationOrientation(target.position - transform.position);       // Used to choose which walking animation to use
			}
		}

		else
		{
			ChangeState(EnemyState.idle);
			anim.SetBool("wakeUp", false); // Start GoToSleep animation
		}
	}



}
