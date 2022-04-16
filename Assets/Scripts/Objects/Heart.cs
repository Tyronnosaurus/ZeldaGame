using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : PowerUp
{
    public IntValue playerHealth;
    public int amountToIncrease;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            // Increase health (up to the maximum)
            playerHealth.value = Mathf.Min(playerHealth.value + amountToIncrease , playerHealth.InitialValue);

            powerupSignal.Raise();
            Destroy(this.gameObject);
        }
    }

}
