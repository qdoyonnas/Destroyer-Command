using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardSense : MonoBehaviour
{
	public AIControls controls;

	private void OnTriggerEnter(Collider other)
	{
		Hazard hazard = other.GetComponentInParent<Hazard>();
		if( hazard ) {
			controls.hazards.Add(hazard);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Hazard hazard = other.GetComponentInParent<Hazard>();
		if( hazard ) {
			controls.hazards.Remove(hazard);
		}
	}
}
