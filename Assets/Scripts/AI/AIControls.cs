using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControls : Controls
{
    public float aimSpeed = 2f;

    protected AIState activeState;
    protected AIAimerState aimerState;

    public List<Hazard> hazards;
    public List<Ship> targets;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

	protected override void Initialize()
	{
		base.Initialize();

        ship.GetAimer().SetVisible(false);

        hazards = new List<Hazard>();
        targets = new List<Ship>();

        SetupSensors();

        activeState = new SearchingState(this);
        aimerState = new IdleAimerState(this, null);

        activeState.Initialize();
        aimerState.Initialize();
	}

	void SetupSensors()
    {
        HazardSense hazard = ship.transform.Find("HazardSense").gameObject.AddComponent<HazardSense>();
        hazard.controls = this;

        TargetSense target = ship.transform.Find("TargetSense").gameObject.AddComponent<TargetSense>();
        target.controls = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ControlsUpdate();
    }

	protected override void ControlsUpdate()
	{
        for( int i = hazards.Count-1; i >= 0; i-- ) {
            if( !hazards[i] ) {
                hazards.RemoveAt(i);
            }
        }
        for( int i = targets.Count-1; i >= 0; i-- ) {
            if( !targets[i] ) {
                targets.RemoveAt(i);
            }
        }

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
