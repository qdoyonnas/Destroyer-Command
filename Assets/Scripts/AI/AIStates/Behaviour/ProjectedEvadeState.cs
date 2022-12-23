using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectedEvadeState : AIState
{
	const float triggerAngle = 45f;
	const float triggerDistance = 40f;

	Projectile projectile;

	public ProjectedEvadeState(AIControls controls)
		: base(controls)
	{
	}

	public override void Initialize()
	{
		projectile = Trigger(controls);
		if( Random.value < 0.5f ) {
			controls.inputs["turn"] = -1f;
		} else {
			controls.inputs["turn"] = 1f;
		}

		controls.inputs["forward"] = 1f;
	}

	public override void End()
	{
		controls.inputs["forward"] = 0f;
		controls.inputs["turn"] = 0f;
	}

	public override bool CheckStateChange()
	{
		if( controls.hazardSense.objects.Count > 0 ) {
			if( EvadeState.Trigger(controls) ) {
				controls.SetState(new EvadeState(controls));
				return true;
			}
		}

		return false;
	}

	public static Projectile Trigger(AIControls controls, float triggerAngle = triggerAngle, float triggerDistance = triggerDistance)
	{
		Ship ship = controls.GetShip();

		foreach( GameObject obj in controls.projectileSense.objects ) {
			Projectile projectile = obj.GetComponent<Projectile>();
			Vector3 end = projectile.transform.position + (projectile.velocity * (projectile.lifetime * 50));
			Vector3 positionAtTime = controls.transform.position + (controls.transform.forward * ship.GetSpeed() * (projectile.lifetime * 50));
			Vector3 delta = end - positionAtTime;
			if( delta.magnitude < triggerDistance ) {
				float angle = Vector3.Angle(controls.transform.forward, delta);
				if( angle < triggerAngle ) {
					return projectile;
				}
			}
		}
		return null;
	}
}
