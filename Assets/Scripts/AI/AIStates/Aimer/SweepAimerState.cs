using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepAimerState : AIAimerState
{
	float timeoutTimestamp = 0f;
	float timeoutLength = 6f;

	float shotTimestamp = 0f;
	float shotTimingMin = 0.1f;
	float shotTimingMax = 0.3f;

	int shotsTaken = 0;
	int shotCount = 0;
	int shotCountMin = 3;
	int shotCountMax = 10;

	float sweepLength = 0f;
	float sweepLengthMin = 10f;
	float sweepLengthMax = 20f;

	Vector3 offset = new Vector3();
	float offsetRange = 10f;
	float leadingOffset = 1f;

	float distanceThreshold = 3f;

	Vector3 sweepVector;

	public SweepAimerState(AIControls controls, Ship target)
		: base(controls, target)
	{
	}

	override public void Initialize()
	{
		timeoutTimestamp = Time.time + timeoutLength;

		shotCount = Random.Range(shotCountMin, shotCountMax);
		offset = new Vector3(
			Random.Range(-offsetRange, offsetRange),
			0,
			Random.Range(-offsetRange, offsetRange)
		);
		sweepLength = Random.Range(sweepLengthMin, sweepLengthMax) * shotCount;
		sweepVector = CalcSweepVector();
	}

	public override void Update()
	{
		if( CheckStateChange() ) { return; }

		controls.inputs["mainFire"] = 0f;

		Vector3 shotTarget = CalcShotTarget();
		float aimDistance = Vector3.Distance(aimer.transform.position, shotTarget);
		if( aimDistance < distanceThreshold 
			&& Time.time > shotTimestamp 
			&& aimer.InAngle() )
		{
			TakeShot();
		} else {
			MoveAimer(shotTarget);
		}
	}
	Vector3 CalcSweepVector()
	{
		Vector2 targetVel2 = new Vector2(target.transform.forward.x, target.transform.forward.z);
		Vector2 sweepVector2 = Vector2.Perpendicular(targetVel2);
		Vector3 sweepVector = new Vector3(sweepVector2.x, 0, sweepVector2.y) * sweepLength;

		return sweepVector;
	}
	Vector3 CalcShotTarget()
	{
		Vector3 intercept = ship.GetActiveWeapon().CalcIntercept(ship, target, leadingOffset) + offset;
		float shotRatio = (float)shotsTaken / (float)shotCount;
		Vector3 sweepStart = intercept - (sweepVector * 0.5f);
		Vector3 shotTarget = sweepStart + (sweepVector * shotRatio);

		return shotTarget;
	}
	void TakeShot()
	{
		controls.inputs["mainFire"] = 1f;
		shotsTaken++;

		shotTimestamp = Time.time + Random.Range(shotTimingMin, shotTimingMax);
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
