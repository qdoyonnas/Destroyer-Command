using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectShotAimerState : AIAimerState
{
	float timeoutTimestamp = 0f;
	float timeoutLength = 6f;

	float shotTimestamp = 0f;
	float shotTimingMin = 0f;
	float shotTimingMax = 1f;

	int shotsTaken = 0;
	int shotCount = 0;
	int shotCountMin = 1;
	int shotCountMax = 10;

	Vector3 offset = new Vector3();
	float offsetRange = 30f;
	float leadingOffset = 1f;

	float distanceThreshold = 3f;
	float minDangerDistance = 90f;
	float dangerAngle = 20f;

	public DirectShotAimerState(AIControls controls, Ship target)
		: base(controls, target) {}

    override public void Initialize()
	{
		timeoutTimestamp = Time.time + timeoutLength;

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

		Vector3 shotTarget = ship.GetActiveWeapon().CalcIntercept(ship, target, leadingOffset) + offset;
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

	public override bool CheckStateChange()
	{
		if( Time.time > timeoutTimestamp || !target || shotsTaken >= shotCount ) {
			controls.SetAimerState(new IdleAimerState(controls, null));
			return true;
		}

		return false;
	}
}
