using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectShotAimerState : AIAimerState
{
	float shotTimestamp = 0f;
	float shotTimingMin = 0f;
	float shotTimingMax = 1f;

	int shotsTaken = 0;
	int shotCount = 0;
	int shotCountMin = 1;
	int shotCountMax = 10;

	Vector3 offset = new Vector3();
	float offsetRange = 30f;

	float distanceThreshold = 3f;
	float minDangerDistance = 40f;
	float dangerAngle = 20f;

	public DirectShotAimerState(AIControls controls, Ship target)
		: base(controls, target)
	{
	}

    override public void Initialize()
	{
		shotCount = Random.Range(shotCountMin, shotCountMax);
		offset = new Vector3(
			Random.Range(-offsetRange, offsetRange),
			0,
			Random.Range(-offsetRange, offsetRange)
		);
	}

	override public void Update()
	{
		if( CheckStateChange() ) { return; }

		controls.inputs["mainFire"] = 0f;

		Vector3 shotTarget = CalcIntercept() + offset;
		float aimDistance = Vector3.Distance(aimer.transform.position, shotTarget);
		if( aimDistance < distanceThreshold 
			&& Time.time > shotTimestamp 
			&& aimer.InAngle()
			&& !CheckIsDanger(shotTarget) )
		{
			TakeShot();
		} else {
			MoveAimer(shotTarget);
		}
	}

	bool CheckIsDanger(Vector3 shotTarget)
	{
		Vector3 delta = shotTarget - controls.transform.position;
		if( delta.magnitude < minDangerDistance ) {
			float angle = Vector3.Angle(controls.transform.forward, delta);
			if( angle < dangerAngle ) {
				return true;
			}
		}

		return false;
	}

	Vector3 CalcIntercept()
	{
		float shotVelocity = ship.GetActiveWeapon().shotVelocity;

		Vector3 delta = ship.transform.position - target.transform.position;
		float distance = delta.magnitude;
		
		float angle = Vector3.Angle(target.transform.forward, delta);
		float timeToTarget = (distance / shotVelocity) * Mathf.Max(1, angle / 45);
		Vector3 targetVelocity = target.transform.forward * target.GetSpeed();
		Vector3 intercept = target.transform.position + (targetVelocity * timeToTarget);

		return intercept;
	}
	void TakeShot()
	{
		controls.inputs["mainFire"] = 1f;
		shotsTaken++;

		shotTimestamp = Time.time + Random.Range(shotTimingMin, shotTimingMax);
		
		offset = new Vector3(
			Random.Range(-offsetRange, offsetRange),
			0,
			Random.Range(-offsetRange, offsetRange)
		);
	}
	void MoveAimer(Vector3 shotTarget)
	{
		Vector3 aimDelta = shotTarget - aimer.transform.position;
		if( aimDelta.x < 0 ) {
			controls.inputs["aimHorizontal"] = -controls.aimSpeed;
		} else if( aimDelta.x > 0 ) {
				controls.inputs["aimHorizontal"] = controls.aimSpeed;
		} else {
			controls.inputs["aimHorizontal"] = 0f;
		}
		if( aimDelta.z < 0 ) {
			controls.inputs["aimVertical"] = -controls.aimSpeed;
		} else if( aimDelta.z > 0 ) {
				controls.inputs["aimVertical"] = controls.aimSpeed;
		} else {
			controls.inputs["aimVertical"] = 0f;
		}
	}

	public override bool CheckStateChange()
	{
		if( !target || shotsTaken >= shotCount ) {
			controls.SetAimerState(new IdleAimerState(controls, null));
			return true;
		}

		return false;
	}

	override public void End()
	{
		controls.inputs["mainFire"] = 0f;
		controls.inputs["aimVertical"] = 0f;
		controls.inputs["aimHorizontal"] = 0f;
	}
}
