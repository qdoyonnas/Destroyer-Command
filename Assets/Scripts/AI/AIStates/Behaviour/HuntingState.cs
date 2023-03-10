using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingState : AIState
{
	Ship target = null;

	float adjustTimingMin = 0.5f;
	float adjustTimingMax = 2f;
	float adjustTimestamp = 0f;

	Vector3 targetOffset = new Vector3();
	float offsetRange = 100f;

	float angleThreshold = 5f;

	public HuntingState(AIControls controls)
		: base(controls)
	{
	}

    override public void Initialize()
	{
		target = Trigger(controls);
		SetOffset();
	}

	override public void Update()
	{
		if( CheckStateChange() ) { return; }

		
		if( Time.time >= adjustTimestamp ) {
			SetOffset();
		}

		Vector3 targetPosition = target.transform.position + targetOffset;
		Vector3 delta = targetPosition - ship.transform.position;
		float angle = Vector3.SignedAngle(ship.transform.forward, delta, Vector3.up);
		if( angle < -angleThreshold ) {
			controls.inputs["turn"] = -1f;
		} else if( angle > angleThreshold ) {
			controls.inputs["turn"] = 1f;
		} else {
			controls.inputs["turn"] = 0f;
		}

		float targetSpeed = ship.maxSpeed - (ship.maxSpeed * (Mathf.Abs(angle) / 180));
		if( ship.GetSpeed() > targetSpeed) {
			controls.inputs["forward"] = -1f;
		} else if( ship.GetSpeed() < targetSpeed ) {
			controls.inputs["forward" ] = 1f;
		} else {
			controls.inputs["forward"] = 0f;
		}
	}

	void SetOffset()
	{
		targetOffset = new Vector3(
			Random.Range(-offsetRange, offsetRange),
			0f,
			Random.Range(-offsetRange, offsetRange)
		);
		adjustTimestamp = Time.time + Random.Range(adjustTimingMin, adjustTimingMax);
	}

	public override bool CheckStateChange()
	{
		if( base.CheckStateChange() ) { return true; }

		if( !target ) {
			controls.SetState(new SearchingState(controls));
			return true;
		}

		return false;
	}

	override public void End()
	{
		controls.inputs["forward"] = 0f;
		controls.inputs["turn"] = 0f;
	}

	public static Ship Trigger(AIControls controls)
	{
		foreach(GameObject ship in controls.targetSense.objects) {
			Controls targetControls = ship.GetComponent<Controls>();
			if( targetControls && targetControls.team != controls.team ) {
				return ship.GetComponent<Ship>();
			}
		}

		return null;
	}
}
