using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ship))]
public class PlayerControls : Controls
{
    public int playerNumber = 0;
    public float aimMult = 3.2f;
    public bool usingKBM = true;

    Vector3 oldMousePosition;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        base.Initialize();

        if( usingKBM ) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        
        oldMousePosition = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        ControlsUpdate();
    }

	protected override void ControlsUpdate()
	{
        if( ship == null ) { return; }

        if( usingKBM ) {
            HandleKBMInputs();
        } else {
            HandleControllerInputs();
        }

        foreach( string key in inputs.Keys ) {
            ship.inputs[key] = inputs[key];
        }

		base.ControlsUpdate();
	}

	public Ship GetShip()
    {
        return ship;
    }

    void HandleKBMInputs()
    {
        inputs["aimHorizontal"] = Input.GetAxis("Mouse X") * aimMult;
        inputs["aimVertical"] = Input.GetAxis("Mouse Y") * aimMult;

        inputs["forward"] = Input.GetAxis("Vertical");
        inputs["turn"] = Input.GetAxis("Horizontal");

        inputs["centerAim"] = Input.GetKey(KeyCode.Space) ? 1 : 0;

        inputs["mainFire"] = Input.GetMouseButton(0) ? 1 : 0;
    }

    void HandleControllerInputs()
    {
        
    }
}
