using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingState : AIState
{
	Vector3 targetPosition = new Vector3();
	float searchRange = 200f;
	float distanceThreshold = 2f;

	public SearchingState(AIControls controls)
		: base(controls)
	{
	}

    override public void Initialize()
	{
		targetPosition = new Vector3(
			controls.transform.position.x + Random.Range(-searchRange, searchRange),
			0,
			controls.transform.position.z + Random.Range(-searchRange, searchRange)
		);
	}

	override public void Update()
	{
		if( CheckStateChange() ) { return; }

		Vector3 delta = targetPosition - controls.transform.position;
		if( delta.magnitude < distanceThreshold ) {
			Initialize();
		}

		AdjustSpeed();
		AdjustAngle(delta);
	}

	bool CheckStateChange()
	{
		if( controls.hazardSense.hazards.Count > 0 ) {
			if( EvadeState.Trigger(controls) ) {
				controls.SetState(new EvadeState(controls));
				return true;
			}
		}

		if( controls.targetSense.targets.Count > 0 ) {
			if( HuntingState.Trigger(controls) ) {
				controls.SetState(new HuntingState(controls));
				return true;
			}
		}

		return false;
	}

	void AdjustSpeed()
	{
		float targetSpeed = ship.maxSpeed - ship.minSpeed;
		if( ship.GetSpeed() < targetSpeed - 0.1f ) {
			controls.inputs["forward"] = 1;
		} else if ( ship.GetSpeed() > targetSpeed + 0.1f ) {
			controls.inputs["forward"] = -1;
		} else {
			controls.inputs["forward"] = 0;
		}
	}
	void AdjustAngle(Vector3 delta)
	{
		float angle = Vector3.SignedAngle(controls.transform.forward, delta, Vector3.up);
		if( angle < -5 ) {
			controls.inputs["turn"] = -1;
		} else if( angle >= 5 ) {
			controls.inputs["turn"] = 1;
		} else {
			controls.inputs["turn"] = 0;
		}
	}

	override public void End()
	{
		controls.inputs["turn"] = 0f;
		controls.inputs["forward"] = 0f;
	}
}
