using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Ship))]
public class PlayerControls : Controls
{
    public int playerNumber = 0;
    public float aimMult = 1.5f;
    
    private PlayerInput playerInput;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = gameObject.AddComponent<PlayerInput>();
        InputActionAsset actionAsset = Resources.Load<InputActionAsset>("Inputs/DC_Default_Inputs");
        playerInput.actions = actionAsset;
        playerInput.currentActionMap = actionAsset.FindActionMap("Player");
        playerInput.neverAutoSwitchControlSchemes = true;
    }

	private void Start()
	{
        Initialize();
	}

	protected override void Initialize()
    {
        base.Initialize();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        ControlsUpdate();
    }

	protected override void ControlsUpdate()
	{
        if( ship == null ) { return; }

        HandleInputs();

        foreach( string key in inputs.Keys ) {
            ship.inputs[key] = inputs[key];
        }

		base.ControlsUpdate();
	}

	public Ship GetShip()
    {
        return ship;
    }

    void HandleInputs()
    {
        lastInputs = new Dictionary<string, float>(inputs);

        Vector2 lookInput = playerInput.currentActionMap.FindAction("Look").ReadValue<Vector2>();
        inputs["aimHorizontal"] = lookInput.x * aimMult; 
        inputs["aimVertical"] = lookInput.y * aimMult;

        Vector2 moveInput = playerInput.currentActionMap.FindAction("Move").ReadValue<Vector2>();
        inputs["forward"] = moveInput.y;
        inputs["turn"] = moveInput.x;

        inputs["centerAim"] = playerInput.currentActionMap.FindAction("Center").triggered ? 1 : 0;

        inputs["mainFire"] = playerInput.currentActionMap.FindAction("Fire").ReadValue<float>() >= 0.5f ? 1 : 0;
    }
}
