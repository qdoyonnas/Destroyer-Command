using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayButton : MenuItem
{
	public string sceneName;

	private void Awake()
	{
		Init();
	}

	public override bool OnSubmit(InputAction.CallbackContext context)
	{
		if( context.performed ) {
			RootManager.instance.SwapMenu(sceneName);
			RootManager.instance.CloseAllScenes();
			SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
			return true;
		}

		return false;
	}
}
