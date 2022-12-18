using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAimerState : AIAimerState
{
	float attackTimestamp = 0f;
	float attackTimeMin = 0f;
	float attackTimeMax = 2f;

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

		if( controls.targets.Count > 0 ) {
			float smallestDistance = -1f;
			Ship closestShip = null;
			foreach( Ship ship in controls.targets ) {
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
				StartAttackPattern(closestShip);
			}
		}
	}

	void StartAttackPattern(Ship ship)
	{
		controls.SetAimerState(new DirectShotAimerState(controls, ship));
	}

	override public void End()
	{

	}
}
