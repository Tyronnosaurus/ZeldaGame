using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float thrust;
	public float knockTime;
	public int damage;

	private void OnTriggerEnter2D(Collider2D other)
	{
		// For breaking pots
		if (other.gameObject.CompareTag("breakable"))
		{
			other.GetComponent<Pot>().Smash();
		}

		// For hitting enemies
		else if (other.gameObject.CompareTag("enemy"))
		{
			Vector3 playerPos = GetComponentInParent<Transform>().position;
			other.GetComponent<Enemy>().Knock(playerPos, thrust, knockTime, damage ); // Execute the enemy's own knockback code
		}

		// For hitting the player (used by enemies)
		else if (other.gameObject.CompareTag("Player"))
		{
			Vector3 attackerPos = GetComponentInParent<Transform>().position;
			other.GetComponent<PlayerMovement>().Knock(attackerPos, thrust, knockTime); // Execute the player's own knockback code
		}
	}


}

