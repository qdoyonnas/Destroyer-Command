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

	virtual public void Initialize()
	{

	}

	virtual public void Update()
	{

	}

	virtual public bool CheckStateChange()
	{
		return false;
	}

	virtual public void End()
	{

	}
}
