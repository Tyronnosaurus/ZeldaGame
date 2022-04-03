using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Shows/hides the context clue (AKA buble) over the player's head
public class ContextClue : MonoBehaviour
{
    public GameObject contextClue;
	public bool contextActive = false;

    public void Enable()
    { contextClue.SetActive(true); }

    public void Disable()
    { contextClue.SetActive(false); }

}
