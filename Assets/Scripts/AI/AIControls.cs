using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControls : Controls
{
    public float aimSpeed = 4f;

    public HazardSense hazardSense;
    public TargetSense targetSense;

    protected AIState activeState;
    protected AIAimerState aimerState;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

	protected override void Initialize()
	{
		base.Initialize();

        ship.GetAimer().SetVisible(false);

        SetupSensors();

        activeState = new SearchingState(this);
        aimerState = new IdleAimerState(this, null);

        activeState.Initialize();
        aimerState.Initialize();
	}

	void SetupSensors()
    {
        hazardSense = ship.transform.Find("HazardSense").gameObject.AddComponent<HazardSense>();
        hazardSense.controls = this;

        targetSense = ship.transform.Find("TargetSense").gameObject.AddComponent<TargetSense>();
        targetSense.controls = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ControlsUpdate();
    }

	protected override void ControlsUpdate()
	{
        activeState.Update();
        aimerState.Update();

		base.ControlsUpdate();
	}

	public void SetState(AIState state)
    {
        activeState.End();
        activeState = state;
        activeState.Initialize();
    }

    public void SetAimerState(AIAimerState state)
    {
        aimerState.End();
        aimerState = state;
        aimerState.Initialize();
    }

    public Ship GetShip()
    {
        return ship;
    }
    public Aimer GetAimer()
    {
        return ship.GetAimer();
    }
}
