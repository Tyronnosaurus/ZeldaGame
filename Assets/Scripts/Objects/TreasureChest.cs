using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureChest : Interactable
{
    [Header("Contents")]
    public Item contents;
    public bool isOpen;
    public Inventory playerInventory;
    public BoolValue storedOpen;

    [Header("Signals & dialog")]
    public Signal raiseItem;
    public GameObject dialogBox;
    public Text dialogText;

    [Header("Animation")]
    private Animator anim;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isOpen = storedOpen.value;
        if (isOpen) anim.SetTrigger("openQuick");
    }


    // Update is called once per frame
    void Update()
    {
        //If player is in range (monitored by OnTriggerEnter/Exit2D) and user presses action button -> Start chest opening process
        if (playerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            if (!isOpen)  OpenChest();
			else          FinishOpeningChest();
        }
    }


    public void OpenChest()
    {
        // Play 'opening chest' animation
        anim.SetTrigger("open");

        // Show dialog window
        dialogBox.SetActive(true);
        dialogText.text = contents.itemDescription;

        // Add contents to inventory
        playerInventory.AddItem(contents);
        playerInventory.currentItem = contents; //To know what to show over player's head

        // Raise signal so that the player does stuff (show item over its head)
        raiseItem.Raise();

        // Raise signal to hide the context clue (would interfere with showing item over player's head)
        contextOff.Raise();

        // Set the chest as already open
        isOpen = true;
        storedOpen.value = true;
    }


    public void FinishOpeningChest()
	{
        // Turn the dialog off
        dialogBox.SetActive(false);
        // Raise the signal to the player to stop animating
        raiseItem.Raise();
    }


    /// <summary>
    /// 
    /// (Overriden from Interactable)
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpen) return; // No need to do anything if chest already open

        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInRange = true;
            contextOn.Raise();  // Raise signal (to show context cue above player's head)
        }
    }

    // Override from Interactable
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInRange = false;
            contextOff.Raise(); // Raise signal (to hide context cue above player's head)
        }
    }

}
