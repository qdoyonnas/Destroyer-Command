using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAimerState : AIAimerState
{
	float attackTimestamp = 0f;
	float attackTimeMin = 0f;
	float attackTimeMax = 2f;

	float forceDirectShotDistance = 20f;

	public IdleAimerState(AIControls controls, Ship target)
		: base(controls, target)
	{

	}

	override public void Initialize()
	{
		attackTimestamp = Random.Range(attackTimeMin, attackTimeMax);
	}

	override public void Update()
	{
		int charges = ship.GetActiveWeapon().charges;
		if( charges == 0 
			|| (charges != ship.GetActiveWeapon().maxCharges && Time.time < attackTimestamp) ) {
			return;
		}

		if( controls.targetSense.objects.Count > 0 ) {
			float smallestDistance = -1f;
			GameObject closestShip = null;
			foreach( GameObject ship in controls.targetSense.objects ) {
				Controls targetControls = ship.GetComponent<Controls>();
				if( targetControls && targetControls.team != controls.team ) {
					float distance = Vector3.Distance(ship.transform.position, controls.transform.position);
					if( closestShip == null || distance < smallestDistance ) {
						smallestDistance = distance;
						closestShip = ship;
					}
				}
			}

			if( closestShip ) {
				StartAttackPattern(closestShip.GetComponent<Ship>());
			}
		}
	}

	void StartAttackPattern(Ship target)
	{
		float distance = Vector3.Distance(target.transform.position, ship.transform.position);
		if( distance < forceDirectShotDistance ) {
			controls.SetAimerState(new DirectShotAimerState(controls, target));
		} else {
			controls.SetAimerState(new SweepAimerState(controls, target));
		}
	}

	override public void End()
	{

	}
}
