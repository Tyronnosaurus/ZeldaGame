using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New IntValue", menuName = "ScriptableObjects/BasicTypes/IntValue")]
public class IntValue : ScriptableObject, ISerializationCallbackReceiver
{
	public int InitialValue;

	public int value;

	// By inheriting from ISerializationCallbackReceiver and adding these 2 functions, we make sure value is reset between executions.
	public void OnAfterDeserialize() { value = InitialValue; }

	public void OnBeforeSerialize() { }
}
