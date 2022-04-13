using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DoorType
{
    key,
    enemy,
    button
}

public class Door : Interactable
{
    [Header("Door variables")]
    public DoorType doorType;
    public bool open;

    public Inventory playerInventory;
    private SpriteRenderer doorSprite;
    private BoxCollider2D boxCollider2D;        // Used for context clue
    private BoxCollider2D boxCollider2D_parent; // Used for opening door


	private void Start()
	{
        playerInventory.numberOfKeys++;
        doorSprite = GetComponentInParent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        //GetComponentInParent() looks inside the calling gameObject before looking inside its parent. So we do this:
        boxCollider2D_parent = transform.parent.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            if (playerInRange && doorType == DoorType.key)
                if (playerInventory.numberOfKeys > 0)
                {
                    Open();
                    playerInventory.numberOfKeys--;
                }
    }


    public void Open()
	{
        doorSprite.enabled = false;             // Turn off the door
        open = true;                            // Set open to true
        boxCollider2D_parent.enabled = false;   // Turn off the door's box collider
        boxCollider2D.enabled = false;          // Turn off boxCollider for context clue

    }


    public void Close()
	{

	}
}
