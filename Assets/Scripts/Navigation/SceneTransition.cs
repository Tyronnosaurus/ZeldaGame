using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransition : MonoBehaviour
{
	[Header("New scene variables")]
    public string SceneToLoad;
	
	/// <summary> Spawn position in the new scene </summary>
	public Vector2 playerSpawnPosition;
	public Vector2 playerSpawnOrientation;

	/// <summary> Scriptable object containing the player's spawn position. Can be changed whenever we go to a new scene. </summary>
	public PlayerSpawnStorage playerSpawnStorage;

	[Header("Transition animation")]
	/// <summary> Animation for when exiting a scene </summary>
	public GameObject fadeOutPanel;
	/// <summary> Animation for when entering a scene </summary>
	public GameObject fadeInPanel;



	/// <summary> Triggers scene transition </summary>
	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !other.isTrigger)	// Player has multiple colliders. Use only the one that isn't trigger.
		{
			playerSpawnStorage.position    = playerSpawnPosition;  // Changes on the Scriptable Object survive between scenes -> When Player loads on a new scene he'll appear in this position
			playerSpawnStorage.orientation = playerSpawnOrientation;
			StartCoroutine(FadeCo());
		}
	}


	// Runs when script instance is loaded -> Fades screen from black to normal
	private void Awake()
	{
		if (fadeInPanel != null) {
			GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
			Destroy(panel, 1); // Program its destruction after 1s
		}
	}


	// Runs when triggering a scene transition collider -> Fades screen to black, and loads scene
	public IEnumerator FadeCo()
	{
		if (fadeOutPanel != null) //Make sure we've assigned the object in the editor
		{
			// Get clip length in seconds. We need to navigate to the Prefab > Child (Panel) > Animator > Clip 0 (the first that was added)
			float clipLength = fadeOutPanel.GetComponentInChildren<Animator>().runtimeAnimatorController.animationClips[0].length;

			// Start instance of the "Fade out" prefab, and waituntil it finishes
			Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
			yield return new WaitForSeconds(clipLength);
		}
		
		// Load goal scene
		//AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneToLoad);
		//while (!asyncOperation.isDone)	yield return null;
		SceneManager.LoadScene(SceneToLoad);
	}


}
