using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMovement : MonoBehaviour
{
    public Transform target;    // Target to follow
    public float smoothing;     // How fast we want to follow [0-1]

    public GameObject InitialRoom;

    private Bounds camBounds;   // Defines bounding box where camera can move so as to not show anything outside the map
    private float camHalfHeight, camHalfWidth;


    // Start is called before the first frame update
    void Start()
    {
        camHalfHeight = Camera.main.orthographicSize; // OrthographicSize gives distance from camera's center to top of the screen
        camHalfWidth  = Camera.main.aspect * camHalfHeight;

        // Camera bounds are usually updated when moving between rooms, but we need to do it at the start for the initial room as well
        UpdateBounds(InitialRoom.GetComponentInChildren<TilemapRenderer>().bounds);

        // Center camera on player immediately at start
        SetOnPlayer();
    }



    void FixedUpdate()
    {
        FollowPlayerSmooth();
    }



    // Centers camera on player instantaneously, but making sure not to show outside of the map if player is near the edge
	private void SetOnPlayer()
	{
        // Camera's Start() runs before Player gets moved to its start position.
        // Therefore we fetch the configured start position (regardless of the player being there or not), rather than target.position.
        Vector2 targetCorrected = target.GetComponent<PlayerMovement>().startingPosition.value;

        float x = Mathf.Clamp(targetCorrected.x, camBounds.min.x, camBounds.max.x);
        float y = Mathf.Clamp(targetCorrected.y, camBounds.min.y, camBounds.max.y);
        transform.position = new Vector3(x, y, transform.position.z);
    }


    // Follow the player with a smooth movement
    private void FollowPlayerSmooth()
	{
        if (transform.position != target.position)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);   // Follow the target's X & Y, but not the Z

            targetPosition.x = Mathf.Clamp(targetPosition.x, camBounds.min.x, camBounds.max.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, camBounds.min.y, camBounds.max.y);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);

            // Rather than having the camera approach the player assimptotically forever, force it onto the player once it gets close enough
            if (Vector3.Distance(transform.position, targetPosition) < 0.01) transform.position = targetPosition;
        }
    }


    // Given the bounds of a map (a tileset), define the new bounds of the camera so that it doesn't show anything outside the map
    public void UpdateBounds(Bounds newRoomBounds)
	{
        // Camera's bounding box is just the map's bounding box with an offset of half height/width inwards
        Vector3 min = new Vector3(newRoomBounds.min.x + camHalfWidth, newRoomBounds.min.y + camHalfHeight, 0); // Lower left corner
        Vector3 max = new Vector3(newRoomBounds.max.x - camHalfWidth, newRoomBounds.max.y - camHalfHeight, 0); // Upper right corner
        camBounds.SetMinMax(min,max);
    }

}
