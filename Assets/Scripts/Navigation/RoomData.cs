using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomData : MonoBehaviour
{
    public string RoomName;
    public bool needsNameShown;



	private void Start()
	{
		// Tilemap bounds do not resize down when erasing tiles. We need to use the editor's tool "Compress Tilemap Bounds" or execute CompressBounds()
		// Necessary since the camera uses these bounds to not show anything off-map.
		GetComponentInChildren<Tilemap>().CompressBounds();
	}

}
