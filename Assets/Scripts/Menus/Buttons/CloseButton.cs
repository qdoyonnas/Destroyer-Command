using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CloseButton : MenuItem
{
	public string sceneName;

	private void Awake()
	{
		Init();
	}

	public override bool OnSubmit(InputAction.CallbackContext context)
	{
		if( context.performed ) {
			SceneManager.UnloadSceneAsync(sceneName);
			return true;
		}

		return false;
	}
}
