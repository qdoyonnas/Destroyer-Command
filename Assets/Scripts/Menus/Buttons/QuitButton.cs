using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuitButton : MenuItem
{
    void Awake()
    {
        Init();
    }

	public override bool OnSubmit(InputAction.CallbackContext context)
	{
		if( context.performed ) {
			Application.Quit();
			return true;
		}
		 
		return false;
	}
}
