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

	}

	virtual public void End()
	{

	}
}
