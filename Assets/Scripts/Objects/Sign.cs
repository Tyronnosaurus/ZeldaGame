using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Sign : Interactable
{
	public GameObject dialogBox;
	public string dialog;
	public Text dialogText;


	// Start is called before the first frame update
	void Start()
    {
		dialogText.text = dialog;
	}


    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space) && playerInRange)
		{
			ToggleDialog();
		}
	}



	private void ToggleDialog()
	{
		dialogBox.SetActive(!dialogBox.activeInHierarchy);    // Toggle dialogBox's 'Active' property
	}


	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			contextOff.Raise(); // Raise signal (to hide context cue above player's head)
			playerInRange = false;
			dialogBox.SetActive(false); // Hide dialog automatically if player walks away
		}
	}


}
