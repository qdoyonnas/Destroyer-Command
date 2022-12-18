using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public Team team;
    public Dictionary<string, float> inputs;
    
    protected Ship ship;

    protected virtual void Initialize()
    {
        inputs = new Dictionary<string, float>();
        inputs["forward"] = 0;
        inputs["turn"] = 0;
        inputs["aimVertical"] = 0;
        inputs["aimHorizontal"] = 0;
        inputs["centerAim"] = 0;
        inputs["mainFire"] = 0;

        ship = gameObject.GetComponent<Ship>();
        ship.SetHullColor(team.hullColor);
        ship.SetTrailColor(team.trailColor);
    }

    // Update is called once per frame
    protected virtual void ControlsUpdate()
    {
        foreach( string key in inputs.Keys ) {
            ship.inputs[key] = inputs[key];
        }
    }
}
