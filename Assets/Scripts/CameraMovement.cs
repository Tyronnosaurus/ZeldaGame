using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMovement : MonoBehaviour
{
    public Transform target;    // Target to follow
    public float smoothing;     // How fast we want to follow [0-1]

    public GameObject InitialRoom;
    private Bounds bounds;
    private float camHeight, camWidth;


    // Start is called before the first frame update
    void Start()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth  = Camera.main.aspect * camHeight;

        UpdateBounds(InitialRoom.GetComponentInChildren<TilemapRenderer>().bounds);

        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);  // Center on player immediately at start
    }


    // LateUpdate is called once per frame, at the end
    void LateUpdate()
    {
        if(transform.position != target.position)
		{
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);   // Follow the target's X & Y, but not the Z
            
            targetPosition.x = Mathf.Clamp(targetPosition.x, bounds.min.x, bounds.max.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, bounds.min.y, bounds.max.y);

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
        bounds.SetMinMax(min,max);
    }
}
