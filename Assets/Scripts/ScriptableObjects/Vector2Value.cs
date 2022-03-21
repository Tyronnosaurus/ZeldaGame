using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Vector2Value : ScriptableObject, ISerializationCallbackReceiver
{
	public Vector2 InitialValue;    // Position to spawn at the start of the game

	[HideInInspector]
	public Vector2 value;	// Every time we change the scene, this gets updated with the new spawn position


	// By inheriting from ISerializationCallbackReceiver and adding these 2 functions, we make sure value is reset between executions.
	public void OnAfterDeserialize() { value = InitialValue; }

	public void OnBeforeSerialize() { }

}
