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
    public EnemyState currentState;
    public int health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
	private Rigidbody2D myRigidBody;



	public void Knock(Vector3 attackerPosition, float thrust, float knockTime)
	{
		StartCoroutine(KnockCo(attackerPosition, thrust, knockTime));
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

}
