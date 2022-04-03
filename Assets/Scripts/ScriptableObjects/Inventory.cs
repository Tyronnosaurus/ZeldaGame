using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
	/// <summary> Item we just obtained (to show over player's head) </summary>
	public Item currentItem;

	/// <summary> Items in inventory </summary>
	public List<Item> items = new List<Item>();

	/// <summary> Number of small keys (they're not stored in inventory like normal items) </summary>
	public int numberOfKeys;


	/// <summary> Add item to inventory </summary>
	public void AddItem(Item itemToAdd)
	{
		// Keys are processed differently from normal items
		if (itemToAdd.isKey)
			numberOfKeys++;
		else
			if (!items.Contains(itemToAdd))   items.Add(itemToAdd);
	}

}
