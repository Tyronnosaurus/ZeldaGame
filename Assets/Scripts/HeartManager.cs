using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HeartManager : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfFullHeart;
    public Sprite emptyHeart;
    public IntValue heartContainers;
    public IntValue playerCurrentHealth;


    // Start is called before the first frame update
    void Start()
    {
        InitHearts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InitHearts()
	{
        for (int i=0; i<heartContainers.InitialValue; i++)
		{
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHeart;
		}
	}


    public void UpdateHearts()
	{
        for (int i=0; i < heartContainers.InitialValue; i++)
		{
            if      (playerCurrentHealth.value >= 2*(i+1))   hearts[i].sprite = fullHeart;     // Full heart
            else if (playerCurrentHealth.value <= 2*i)        hearts[i].sprite = emptyHeart;    // Empty heart
			else                                             hearts[i].sprite = halfFullHeart; // Half heart
        }
	}

}

