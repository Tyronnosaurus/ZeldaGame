using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoomMove : MonoBehaviour
{

    public Vector2 newMinPos, newMaxPos;    // Bounding box of the target room
    public Vector2 playerChange;            // Small push to the player to move inside the room
    private CameraMovement cam;

    public bool needText;
    public string placeName;
    public GameObject text;
    public Text placeText;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CameraMovement>();   // Get camera
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cam.minPosition = newMinPos;
            cam.maxPosition = newMaxPos;
            other.transform.position += (Vector3)playerChange;

			if (needText)
			{
                StartCoroutine(placeNameCo());
			}
        }
    }


    private IEnumerator placeNameCo()
	{
        text.SetActive(true);
        placeText.text = placeName;
        yield return new WaitForSeconds(4f);
        text.SetActive(false);
	}
}
