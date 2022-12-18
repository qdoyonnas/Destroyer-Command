using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardSense : MonoBehaviour
{
	public AIControls controls;
	public List<Hazard> hazards;

	private void Start()
	{
		hazards = new List<Hazard>();
	}

	private void Update()
	{
		for( int i = hazards.Count-1; i >= 0; i-- ) {
            if( !hazards[i] ) {
                hazards.RemoveAt(i);
            }
        }
	}

	private void OnTriggerEnter(Collider other)
	{
		Hazard hazard = other.GetComponentInParent<Hazard>();
		if( hazard ) {
			hazards.Add(hazard);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Hazard hazard = other.GetComponentInParent<Hazard>();
		if( hazard ) {
			hazards.Remove(hazard);
		}
	}
}
