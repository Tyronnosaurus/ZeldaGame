using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CoinTextManager : MonoBehaviour
{
	public Inventory playerInventory;
	public TextMeshProUGUI coinCounter;


	private void Start()
	{
		UpdateCoinCounter();
	}


	public void UpdateCoinCounter()
	{
		coinCounter.text = playerInventory.coins.ToString();
	}

}
