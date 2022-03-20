using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


public class RoomMove : MonoBehaviour
{
    public Vector2 playerChange;            // Small push to the player to move inside the room
    
    private CameraMovement cam;

    private Bounds mapBounds;

    public GameObject targetRoom;
    public Tilemap targetGround;

    public GameObject text;
    public Text placeText;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CameraMovement>();   // Get camera

        // Tilemap bounds do not resize down when erasing tiles. We need to use the editor's tool "Compress Tilemap Bounds" or execute CompressBounds()
        targetRoom.GetComponentInChildren<Tilemap>().CompressBounds();
        mapBounds = targetRoom.GetComponentInChildren<TilemapRenderer>().bounds;
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Give the player a little push 
            other.transform.position += (Vector3)playerChange;

            // Reconfigure camera limits
            cam.UpdateBounds(mapBounds);

            // Show name of new room
            if (targetRoom.GetComponent<RoomData>().needsNameShown)   StartCoroutine(placeNameCo());
        }
    }


    private IEnumerator placeNameCo()
	{
        text.SetActive(true);
        placeText.text = targetRoom.GetComponent<RoomData>().RoomName;
        yield return new WaitForSeconds(4f);
        text.SetActive(false);
	}
}
