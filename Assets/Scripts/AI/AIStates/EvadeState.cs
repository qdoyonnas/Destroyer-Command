using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : AIState
{
	const float triggerAngle = 30f;
	const float triggerDistance = 80f;

	float wideTriggerAngle = 65f;

	Hazard hazard;

	public EvadeState(AIControls controls)
		: base(controls)
	{
	}

	override public void Initialize()
	{
		hazard = Trigger(controls, wideTriggerAngle);
		Vector3 delta = hazard.transform.position - ship.transform.position;
		float angle = Vector3.SignedAngle(ship.transform.forward, delta, Vector3.up);
		if( angle < 0f ) {
			controls.inputs["turn"] = 1f;
		} else {
			controls.inputs["turn"] = -1f;
		}
	}

	override public void Update()
	{
		if( CheckStateChange() ) { return; }

		Vector3 delta = hazard.transform.position - ship.transform.position;
		float angle = Vector3.SignedAngle(ship.transform.forward, delta, Vector3.up);
		float targetSpeed = ship.maxSpeed * (Mathf.Abs(angle) / 180);
		if( ship.GetSpeed() > targetSpeed) {
			controls.inputs["forward"] = -1f;
		} else if( ship.GetSpeed() < targetSpeed ) {
			controls.inputs["forward" ] = 1f;
		} else {
			controls.inputs["forward"] = 0f;
		}
	}

	bool CheckStateChange()
	{
		bool evade = true;
		if( !hazard || RaycastAgainstHazard(controls, hazard) == null ) {
			evade = false;
		}

		if( !evade ) {
			controls.SetState(new SearchingState(controls));
			return true;
		}

		return false;
	}

	override public void End()
	{
		controls.inputs["turn"] = 0f;
		controls.inputs["forward"] = 0f;
	}

	public static Hazard RaycastAgainstHazard(AIControls controls, Hazard hazard)
	{
		Ship ship = controls.GetShip();

		Vector3 delta = hazard.transform.position - ship.transform.position;
		Vector3 deltaNorm = delta.normalized;
		float angle = Vector3.SignedAngle(ship.transform.forward, delta, Vector3.up);

		Vector3 triggerVector;
		if( angle < triggerAngle ) {
			triggerVector = deltaNorm;
		} else {
			triggerVector = Vector3.RotateTowards(controls.transform.forward, deltaNorm, Mathf.Deg2Rad * triggerAngle, 0f);
		}

		RaycastHit[] hits = Physics.RaycastAll(controls.transform.position, triggerVector, triggerDistance);
		foreach( RaycastHit hit in hits ) {
			Hazard hitHazard = hit.collider.GetComponentInParent<Hazard>();
			if( hitHazard ) {
				return hitHazard;
			}
		}

		return null;
	}

	public static Hazard Trigger(AIControls controls, float triggerAngle = triggerAngle, float triggerDistance = triggerDistance)
	{
		Ship ship = controls.GetShip();

		foreach( Hazard hazard in controls.hazardSense.hazards ) {
			Hazard hazardest = RaycastAgainstHazard(controls, hazard);
			if( hazardest ) {
				return hazardest;
			}
		}
		return null;
	}
}
