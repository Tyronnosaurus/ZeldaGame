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

    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        // Get dimensions of the camera
        camHalfHeight = Camera.main.orthographicSize; // OrthographicSize gives distance from camera's center to top of the screen (so half the height)
        camHalfWidth  = Camera.main.aspect * camHalfHeight;

        // Camera bounds are usually updated when moving between rooms, but we need to do it at the start of a scene for the initial room as well
        UpdateCamBounds(InitialRoom.GetComponentInChildren<TilemapRenderer>().bounds);

        // Center camera on player immediately (before 1st frame) to prevent initial jump if player and camera had different positions in the editor
        SetOnPlayer();

        animator = GetComponent<Animator>();
    }



    void FixedUpdate()
    {
        FollowPlayerSmooth();
    }



    // Centers camera on player instantaneously
    // If player is near the edge of the map, stops before showing outside the map
	private void SetOnPlayer()
	{
        // Camera's Start() runs before Player gets moved to its start position.
        // Therefore we fetch the configured start position (regardless of the player being there or not), rather than target.position.
        Vector2 targetCorrected = target.GetComponent<PlayerMovement>().playerSpawnStorage.position;

        float x = Mathf.Clamp(targetCorrected.x, camBounds.min.x, camBounds.max.x);
        float y = Mathf.Clamp(targetCorrected.y, camBounds.min.y, camBounds.max.y);
        transform.position = new Vector3(x, y, transform.position.z);
    }


    // Follow the player with a smooth movement
    private void FollowPlayerSmooth()
	{
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);   // Follow the target's X & Y, but not the Z

        if (Vector3.Distance(transform.position, targetPosition) > 0.005)    // Stop following when tolerance is reached
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, camBounds.min.x, camBounds.max.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, camBounds.min.y, camBounds.max.y);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }




    /// <summary>
    /// Given the rectangular bounds of a map (a tileset), define the bounds in which the camera can be so that it doesn't show anything outside the map 
    ///       _______________________
    ///      |MAP_________________   |
    ///      |  | AREA WHERE CAM  |  |  
    ///      |  | CENTER CAN MOVE |--|halfCamWidth
    ///      |  |_________________|  |
    ///      |__________________|____|
    ///                         halfCamHeight
    /// </summary>
    public void UpdateCamBounds(Bounds newRoomBounds)
	{
		// Find bounds of area where camera center can move. It's basically the map's contour minus half width/height of the camera.
		// Camera's bounding box is just the map tileset's bounding box with an offset of half the camera's height/width inwards.
		// Note: Rectangular bounds are represented by points min (lower left) and max (upper right).
		float minX = newRoomBounds.min.x + camHalfWidth;
		float maxX = newRoomBounds.max.x - camHalfWidth;
		float minY = newRoomBounds.min.y + camHalfHeight;
		float maxY = newRoomBounds.max.y - camHalfHeight;

		// Careful! For very small rooms the camera bounds will flip over and cause visual glitches.
		// We just force the camera to stay at the center of the room then (separately for X and Y as needed).
		if (minX > maxX) { minX = maxX = (newRoomBounds.min.x + newRoomBounds.max.x) / 2; }
		if (minY > maxY) { minY = maxY = (newRoomBounds.min.y + newRoomBounds.max.y) / 2; }

		// Build and return the actual bounds
		Vector3 min = new Vector3(minX, minY, 0); // Lower left corner
		Vector3 max = new Vector3(maxX, maxY, 0); // Upper right corner
		camBounds.SetMinMax(min, max);
	}



    /// <summary> Triggers camera shake. Called by signal listener. </summary>
    public void BeginCameraKick()
    {
        animator.SetTrigger("CameraShake");
    }

}
