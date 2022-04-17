using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New BoolValue", menuName = "ScriptableObjects/BasicTypes/BoolValue")]
public class BoolValue : ScriptableObject, ISerializationCallbackReceiver
{
	public bool InitialValue;

	public bool value;

	// By inheriting from ISerializationCallbackReceiver and adding these 2 functions, we make sure value is reset between executions.
	public void OnAfterDeserialize() { value = InitialValue; }

	public void OnBeforeSerialize() { }


}
