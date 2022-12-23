using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAimerState
{
	protected AIControls controls;
	protected Ship ship;
	protected Aimer aimer;
	protected Ship target = null;

	public AIAimerState(AIControls controls, Ship target)
	{
		this.controls = controls;
		this.ship = controls.GetShip();
		this.aimer = ship.GetAimer();
		this.target = target;
	}

	virtual public void Initialize() {}

	virtual public void Update() {}

	virtual public bool CheckStateChange()
	{
		return false;
	}

	virtual public void End()
	{
		controls.inputs["mainFire"] = 0f;
		controls.inputs["aimVertical"] = 0f;
		controls.inputs["aimHorizontal"] = 0f;
	}

	public void MoveAimer(Vector3 shotTarget)
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
}
