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
	}

	override public void Update()
	{
		if( CheckStateChange() ) { return; }

		Vector3 delta = hazard.transform.position - ship.transform.position;
		float angle = Vector3.SignedAngle(ship.transform.forward, delta, Vector3.up);
		if( angle < 0f ) {
			controls.inputs["turn"] = 1f;
		} else {
			controls.inputs["turn"] = -1f;
		}

		float targetSpeed = ship.maxSpeed * (Mathf.Abs(angle) / 180);
		if( ship.GetSpeed() > targetSpeed) {
			controls.inputs["forward"] = -1f;
		} else if( ship.GetSpeed() < targetSpeed ) {
			controls.inputs["forward" ] = 0f;
		} else {
			controls.inputs["forward"] = 0f;
		}
	}

	bool CheckStateChange()
	{
		bool evade = true;
		if( !hazard ) {
			evade = false;
		} else {
			Vector3 delta = hazard.transform.position - ship.transform.position;
			float angle = Vector3.Angle(ship.transform.forward, delta);
			if( angle > wideTriggerAngle ) {
				evade = false;
			}
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
	}

	public static Hazard Trigger(AIControls controls, float triggerAngle = triggerAngle, float triggerDistance = triggerDistance)
	{
		Ship ship = controls.GetShip();

		Hazard hazardest = null;
		float smallestAngle = triggerAngle;
		foreach( Hazard hazard in controls.hazards ) {
			Vector3 delta = hazard.transform.position - ship.transform.position;
			float angle = Vector3.Angle(ship.transform.forward, delta);
			if( angle < triggerAngle && delta.magnitude < triggerDistance ) {
				if( angle < smallestAngle ) {
					hazardest = hazard;
					smallestAngle = angle;
				}
			}
		}

		return hazardest;
	}
}
