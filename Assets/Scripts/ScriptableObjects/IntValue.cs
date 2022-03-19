using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IntValue : ScriptableObject, ISerializationCallbackReceiver
{
	public int InitialValue;

	[HideInInspector]
	public int value;

	// By inheriting from ISerializationCallbackReceiver and adding these 2 functions, we make sure value is reset between executions.
	public void OnAfterDeserialize() { value = InitialValue; }

	public void OnBeforeSerialize() { }
}
