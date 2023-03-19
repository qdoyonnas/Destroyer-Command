using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuSceneButton : MenuItem
{
	public string sceneName;

    void Awake()
    {
        Init();
    }

	public override bool OnSubmit(InputAction.CallbackContext context)
	{
		if( context.performed ) {
			RootManager.instance.SwapMenu(sceneName);
			return true;
		}

		return false;
	}
}
