using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Enemy
{
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    private Animator anim;



    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        currentState = EnemyState.idle;
        target = GameObject.FindWithTag("Player").transform;    //
        anim = GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        CheckDistance();
    }


    void CheckDistance()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= chaseRadius)
        {
            if (currentState == EnemyState.idle)
            {
                ChangeState(EnemyState.walk);  // Player gets close -> Awake
                anim.SetBool("wakeUp", true);
            }

            if ((distance > attackRadius) && (currentState == EnemyState.walk)) {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);  // If awake, chase player
                changeAnimationOrientation( target.position - transform.position );       // Used to choose which walking animation to use
            }
        }

        if (distance > chaseRadius)
        {
            ChangeState(EnemyState.idle);
            anim.SetBool("wakeUp", false); // Start GoToSleep animation
        }

    }


    // Compute which direction is a vector pointing to (by dividing the 2D into 4 quadrants with two diagonals)
    private void changeAnimationOrientation(Vector2 dir)
    {
        /*   \     /      - We divide the 2D plane in 4 parts (cones) separated by diagonals.  
              \ U /       - These areas can be defined by functions. For example, y>|x| is the UP cone. Tip: you can plot the function in Wolphram Alpha.
               \|/        - This is a very efficient approach (only uses <, > and Abs()).
            L---+---R     - Many videogame maps have the player entering from the bottom or the left, meaning enemies will have
               /|\          to look in those directions. For efficiency, these directions are checked first.
              / D \     
             /     \        */

        if      (dir.y <=  Mathf.Abs(dir.x))   animSetFloatsXY( 0 ,-1);    // DOWN    
        else if (dir.x <= -Mathf.Abs(dir.y))   animSetFloatsXY(-1 , 0);    // LEFT 
        else if (dir.y >=  Mathf.Abs(dir.x))   animSetFloatsXY( 0 , 1);    // UP  
        else                                   animSetFloatsXY( 1 , 0);    // RIGHT              
    }

    // Update animator variables so that blend tree uses the correct animation
    private void animSetFloatsXY(float x, float y)
	{
        anim.SetFloat("movX", x);
        anim.SetFloat("movY", y);
    }




    private void ChangeState(EnemyState newState)
	{
        if(currentState != newState)   currentState = newState;
	}
}
