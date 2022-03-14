using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Enemy
{
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.idle;
        target = GameObject.FindWithTag("Player").transform;    //
    }


    void FixedUpdate()
    {
        CheckDistance();
    }


    void CheckDistance()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if ((distance <= chaseRadius) && (distance > attackRadius))
        {
            if (currentState == EnemyState.idle) ChangeState(EnemyState.walk);  // Player gets close -> Awake
            if (currentState == EnemyState.walk) transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);  // If awoken, chase player
        }
	}



    private void ChangeState(EnemyState newState)
	{
        if(currentState != newState)   currentState = newState;
	}
}
