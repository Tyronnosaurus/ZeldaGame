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
	public IntValue maxHealth;	// Scriptable object
    public int health = 1;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
	private Rigidbody2D myRigidBody;


	public virtual void Start()
	{
		health = maxHealth.InitialValue;
	}

	public void Knock(Vector3 attackerPosition, float thrust, float knockTime, int damage)
	{
		TakeDamage(damage); 
		if (this.gameObject.activeInHierarchy) StartCoroutine(KnockCo(attackerPosition, thrust, knockTime));
	}


	private void TakeDamage(int damage)
	{
		health -= damage;
		if (health <= 0) this.gameObject.SetActive(false);
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
