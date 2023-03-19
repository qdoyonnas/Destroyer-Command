using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControls : Controls
{
    public float aimSpeed = 4f;

    public Sense hazardSense;
    public Sense targetSense;
    public Sense projectileSense;

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
        GameObject hazardSenseObject = ship.transform.Find("HazardSense").gameObject;
        hazardSense = hazardSenseObject.AddComponent<Sense>();
        hazardSense.controls = this;
        hazardSense.types = new Sense.SenseType[] { Sense.SenseType.Hazard };

        GameObject targetSenseObject = ship.transform.Find("TargetSense").gameObject;
        targetSense = targetSenseObject.AddComponent<Sense>();
        targetSense.controls = this;
        targetSense.types = new Sense.SenseType[] { Sense.SenseType.Ship };

        projectileSense = hazardSenseObject.AddComponent<Sense>();
        projectileSense.controls = this;
        projectileSense.types = new Sense.SenseType[] { Sense.SenseType.Projectile };

        hazardSenseObject.GetComponent<SphereCollider>().enabled = true;
        targetSenseObject.GetComponent<SphereCollider>().enabled = true;
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
