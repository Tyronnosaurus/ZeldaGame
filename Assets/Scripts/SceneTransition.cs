using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransition : MonoBehaviour
{
    public string SceneToLoad;
	
	public Vector2 playerPosition;
	public Vector2Value playerStorage;


	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !other.isTrigger)
		{
			playerStorage.value = playerPosition;	// Change on the Scriptable Object -> When Player loads on a new scene he'll appear in this position
			SceneManager.LoadScene(SceneToLoad);
		}
	}
}
