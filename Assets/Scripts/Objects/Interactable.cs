using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Signal contextOn, contextOff;

    protected bool playerInRange;	//protected so that derived classes can access it


	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			contextOn.Raise();  // Raise signal (to show context cue above player's head)
			playerInRange = true;
		}
	}


	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			contextOff.Raise(); // Raise signal (to hide context cue above player's head)
			playerInRange = false;
		}
	}
}
