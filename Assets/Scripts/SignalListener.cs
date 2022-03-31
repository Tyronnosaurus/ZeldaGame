using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// Goes in objects and listens for signals


public class SignalListener : MonoBehaviour
{
	public Signal signal;	// A specific signal (an instance of the Signal scriptable object). Chosen in inspector.
	public UnityEvent signalEvent; // What to do when the signal is raised. Configured in inspector.


	// When the signal alerts this listener that its raise, run whichever event is configured on the inspector
	public void OnSignalRaised()
	{ signalEvent.Invoke(); }


	// When this gameObject is enabled, register as listener to the signal (so that it reports back when it's raised)
	private void OnEnable()
	{ signal.RegisterListener(this); }


	// When this gameObject is enabled, deregister
	private void OnDisable()
	{ signal.DeRegisterListener(this); }

}
