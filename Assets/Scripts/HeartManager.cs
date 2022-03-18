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

}

