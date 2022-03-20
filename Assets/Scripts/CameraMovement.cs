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
    private float camHeight, camWidth;


    // Start is called before the first frame update
    void Start()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth  = Camera.main.aspect * camHeight;

        UpdateBounds(InitialRoom.GetComponentInChildren<TilemapRenderer>().bounds);

        SetOnPlayer();  // Center on player immediately at start
    }


    // LateUpdate is called once per frame, at the end
    void LateUpdate()
    {
        FollowPlayerSmooth();
    }


    // Centers camera on player intantaneously, but making not to show outside of the map if player is near the edge
	private void SetOnPlayer()
	{
        float x = Mathf.Clamp(target.position.x, camBounds.min.x, camBounds.max.x);
        float y = Mathf.Clamp(target.position.y, camBounds.min.y, camBounds.max.y);
        transform.position = new Vector3(x, y, transform.position.z);
    }

    private void FollowPlayerSmooth()
	{
        if (transform.position != target.position)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);   // Follow the target's X & Y, but not the Z

            targetPosition.x = Mathf.Clamp(targetPosition.x, camBounds.min.x, camBounds.max.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, camBounds.min.y, camBounds.max.y);

            //if camBounds.Contains(transform.position)
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);

            // Rather than having the camera approach the player assimptotically forever, force it onto the player once it gets close enough
            if (Vector3.Distance(transform.position, targetPosition) < 0.01) transform.position = targetPosition;
        }
    }

    public void UpdateBounds(Bounds newRoomBounds)
	{
        // We got the map's bounding box, but the camera needs to move inside a smaller bounding box (offset by half the camera's width/height) 
        Vector3 min = new Vector3(newRoomBounds.min.x + camWidth , newRoomBounds.min.y + camHeight , 0);
        Vector3 max = new Vector3(newRoomBounds.max.x - camWidth , newRoomBounds.max.y - camHeight , 0);
        camBounds.SetMinMax(min,max);
    }

}
