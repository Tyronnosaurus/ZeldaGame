using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


public class RoomMove : MonoBehaviour
{
    public Vector2 playerChange;    // Direction of small push to the player to move inside the room
    public GameObject targetRoom;

    public GameObject text;
    private Text placeText;

    private CameraMovement cam;
    private Bounds mapBounds;


    // Start is called before the first frame update
    void Start()
    {
        // Get camera
        cam = Camera.main.GetComponent<CameraMovement>();

        // Get tilemap bounds of target room
        mapBounds = targetRoom.GetComponentInChildren<TilemapRenderer>().bounds;

        //
        placeText = text.GetComponent<Text>();
    }



    // Player has touched the Room Transfer collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger) // Player has multiple BoxCollider2D but only check the 'trigger' one
        {
            // Give the player a little push (to prevent triggering the opposite RoomTransfer)
            other.transform.position += (Vector3)playerChange;

            // Reconfigure camera limits
            cam.UpdateCamBounds(mapBounds);

            // Show name of new room
            if (targetRoom.GetComponent<RoomData>().needsNameShown)   StartCoroutine(placeNameCo());
        }
    }


    // Shows an UI overlay with the name of the room for a few seconds
    private IEnumerator placeNameCo()
	{
        text.SetActive(true);
        placeText.text = targetRoom.GetComponent<RoomData>().RoomName;
        yield return new WaitForSeconds(4f);
        text.SetActive(false);
	}
}
