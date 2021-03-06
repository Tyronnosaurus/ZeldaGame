using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}

public class Enemy : MonoBehaviour
{
	[Header("Stats")]
	public IntValue maxHealth;	// Scriptable object
    public int health = 1;
    public int baseAttack;
    public float moveSpeed;
	public float chaseRadius;
	public float attackRadius;

	[Header("Death effects")]
	public GameObject deathEffect;
	private const float deathEffectDelay = 1f;

	// State Machine
	protected EnemyState currentState;

	// Target to chase
	protected Transform target;

	// Components
	protected Animator anim;
	private Rigidbody2D myRigidBody;



	protected virtual void Start()
	{
		health = maxHealth.InitialValue;
		target = GameObject.FindWithTag("Player").transform;
		anim = GetComponent<Animator>();
		currentState = EnemyState.idle;
	}


	protected void Update()
	{
		CheckDistance();
	}


	public void Knock(Vector3 attackerPosition, float thrust, float knockTime, int damage)
	{
		if (currentState != EnemyState.stagger)	// Make enemy invulnerable while being knocked back
		{
			TakeDamage(damage);
			if (this.gameObject.activeInHierarchy) StartCoroutine(KnockCo(attackerPosition, thrust, knockTime));
		}
	}


	private void TakeDamage(int damage)
	{
		health -= damage;
		if (health <= 0)
		{
			this.gameObject.SetActive(false);
			Die();
		}
	}


	private IEnumerator KnockCo(Vector3 attackerPosition, float thrust, float knockTime)
	{
		if (attackerPosition!=null)
		{
			currentState = EnemyState.stagger;
			myRigidBody = GetComponent<Rigidbody2D>();

			// Start moving in the specified direction at the set speed
			Vector2 throwDirection = transform.position - attackerPosition;
			Vector2 speedVector = throwDirection.normalized * thrust;
			myRigidBody.velocity = speedVector;

			// After some time, stop the movement
			yield return new WaitForSeconds(knockTime);
			myRigidBody.velocity = Vector2.zero;

			currentState = EnemyState.idle;
		}
	}




	protected virtual void CheckDistance()
	{
		float distance = Vector3.Distance(target.position, transform.position);

		if (distance <= chaseRadius)
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

		if (distance > chaseRadius)
		{
			ChangeState(EnemyState.idle);
			anim.SetBool("wakeUp", false); // Start GoToSleep animation
		}

	}

	protected void ChangeState(EnemyState newState)
	{
		if (currentState != newState) currentState = newState;
	}




	// Compute which direction is a vector pointing to (by dividing the 2D into 4 quadrants with two diagonals)
	protected void changeAnimationOrientation(Vector2 dir)
	{
		/*   \     /      - We divide the 2D plane in 4 parts (cones) separated by diagonals.  
              \ U /       - These areas can be defined by functions. For example, y>|x| is the UP cone. Tip: you can plot the function in Wolphram Alpha.
               \|/        - This is a very efficient approach (only uses <, > and Abs()).
            L---+---R     - Many videogame maps have the player entering from the bottom or the left, meaning enemies will have
               /|\          to look in those directions. For efficiency, these directions are checked first.
              / D \     
             /     \        */

		if      (dir.y <= -Mathf.Abs(dir.x)) animSetFloatsXY(0, -1);  // DOWN    
		else if (dir.x <= -Mathf.Abs(dir.y)) animSetFloatsXY(-1, 0);  // LEFT
		else if (dir.y >=  Mathf.Abs(dir.x)) animSetFloatsXY(0, 1);   // UP  
		else                                 animSetFloatsXY(1, 0);   // RIGHT              
	}

	// Update animator variables so that blend tree uses the correct animation
	private void animSetFloatsXY(float x, float y)
	{
		anim.SetFloat("movX", x);
		anim.SetFloat("movY", y);
	}



	private void Die()
	{
		if (deathEffect != null)
		{
			GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
			Destroy(effect, deathEffectDelay); //Destroy after some time to let animation finish
		}
	}
}
