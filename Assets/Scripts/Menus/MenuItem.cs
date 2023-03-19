using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuItem : MonoBehaviour
{
	public Vector2Int positionInMenu = new Vector2Int();
	
	GameObject selectionIndicator;

	void Awake()
	{
		Init();
	}
	protected void Init()
	{
		selectionIndicator = transform.Find("Select").gameObject;
		selectionIndicator.SetActive(false);
	}

	public void OnSelect()
	{
		selectionIndicator.SetActive(true);
	}
	public void OnDeselect()
	{
		selectionIndicator.SetActive(false);
	}

	// TODO: Menu items should use Unity Actions or something similar
	// to make use of functionality of other objects
	public virtual bool OnNext(InputAction.CallbackContext context)
	{
		return false;
	}
	public virtual bool OnNavigate(InputAction.CallbackContext context)
	{
		return false;
	}
	public virtual bool OnSubmit(InputAction.CallbackContext context)
	{
		return false;
	}
	public virtual bool OnMenu(InputAction.CallbackContext context)
	{
		return false;
	}
}
