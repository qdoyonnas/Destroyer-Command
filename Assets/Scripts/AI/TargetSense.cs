using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSense : MonoBehaviour
{
	public AIControls controls;
	public List<Ship> targets;

	private void Start()
	{
		targets = new List<Ship>();
	}

	private void OnTriggerEnter(Collider other)
	{
		Ship ship = other.GetComponent<Ship>();
		if( ship && ship != controls.GetShip() ) {
			targets.Add(ship);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Ship ship = other.GetComponent<Ship>();
		if( ship != controls.GetShip() ) {
			targets.Remove(ship);
		}
	}
}
