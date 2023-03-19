using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState
{
	protected AIControls controls;
	protected Ship ship;

	public AIState(AIControls controls)
	{
		this.controls = controls;
		this.ship = controls.GetShip();
	}

	virtual public void Initialize()
	{

	}

	virtual public void Update()
	{
		if( CheckStateChange() ) { return; }
	}

	virtual public bool CheckStateChange()
	{
		if( controls.hazardSense.objects.Count > 0 ) {
			if( EvadeState.Trigger(controls) ) {
				controls.SetState(new EvadeState(controls));
				return true;
			}
		}

		if( controls.projectileSense.objects.Count > 0 ) {
			if( ProjectedEvadeState.Trigger(controls) ) {
				controls.SetState(new ProjectedEvadeState(controls));
				return true;
			}
		}

		return false;
	}

	virtual public void End()
	{

	}

}
