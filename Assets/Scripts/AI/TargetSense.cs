using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSense : MonoBehaviour
{
	public AIControls controls;

	private void OnTriggerEnter(Collider other)
	{
		Ship ship = other.GetComponent<Ship>();
		if( ship && ship != controls.GetShip() ) {
			controls.targets.Add(ship);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Ship ship = other.GetComponent<Ship>();
		if( ship != controls.GetShip() ) {
			controls.targets.Remove(ship);
		}
	}
}
