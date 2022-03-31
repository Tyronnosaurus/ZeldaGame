using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Scriptable Object definition
// Used to create scriptable objects of type Signal (e.g. PlayerAttacked, PlayerIsClose, etc.)
// Using these signals

[CreateAssetMenu]
public class Signal : ScriptableObject
{
	// List of objects that have subscribed (who want to know when the signal is raised)
	public List<SignalListener> listeners = new List<SignalListener>();

	public void Raise()
	{
		// Alert all listeners that the signal is raised.
		// Note: we count backwards because ???
		for(int i = listeners.Count-1; i >= 0; i--)
			listeners[i].OnSignalRaised();
	}


	public void RegisterListener(SignalListener listener)
	{ listeners.Add(listener); }

	public void DeRegisterListener(SignalListener listener)
	{ listeners.Remove(listener); }
}
