using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Sign : MonoBehaviour
{

    public GameObject dialogBox;
    public Text dialogText;
    public string dialog;
    private bool playerInRange;


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
			dialogBox.SetActive( !dialogBox.activeInHierarchy );    // Toggle dialogBox's 'Active' property
		}
    }


	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
            playerInRange = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
            playerInRange = false;
			dialogBox.SetActive(false);
		}
	}

}
