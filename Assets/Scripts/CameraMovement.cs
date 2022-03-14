using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform target;    // Target to follow
    public float smoothing;     // How fast we want to follow [0-1]

    public Vector2 minPosition, maxPosition;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);  // Center on player immediately at start
    }

    // LateUpdate is called once per frame, at the end
    void LateUpdate()
    {
        if(transform.position != target.position)
		{
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);   // Follow the target's X & Y, but not the Z
            
            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);

            // Rather than having the camera approach the player assimptotically forever, force it onto the player once it gets close enough
            if (Vector3.Distance(transform.position, targetPosition) < 0.01) transform.position = targetPosition;
		}
    }
}
