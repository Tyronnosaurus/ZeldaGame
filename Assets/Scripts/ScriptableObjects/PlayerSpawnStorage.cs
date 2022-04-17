using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerSpawnStorage : ScriptableObject, ISerializationCallbackReceiver
{
	// Position to spawn at the start of the game
	[Header("Spawn at the start of the game")]
	public Vector2 InitialPosition;
	public Vector2 InitialOrientation;

	// Every time we change the scene, these get updated with the new spawn
	//[HideInInspector]
	public Vector2 position;
	//[HideInInspector]
	public Vector2 orientation;


	// By inheriting from ISerializationCallbackReceiver and adding these 2 functions, we make sure value is reset between executions.
	public void OnAfterDeserialize() {
		position = InitialPosition;
		orientation = InitialOrientation;
	}

	public void OnBeforeSerialize() { }

}
