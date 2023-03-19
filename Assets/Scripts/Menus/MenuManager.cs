using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public bool closable = false;
	public Vector2Int activeItemPos;
	public MenuItem activeItem {
		get {
			return menuItems[activeItemPos.x, activeItemPos.y];
		}
		set {
			activeItemPos = value.positionInMenu;
		}
	}

	public string[] adjoinedScenes = new string[0];

    public MenuItem[,] menuItems;
	private GraphicRaycaster raycaster;
	private Canvas canvas;

	void Start()
	{
		raycaster = GetComponent<GraphicRaycaster>();
		canvas = GetComponent<Canvas>();
		activeItemPos = new Vector2Int();

		CollectMenuItems();

		activeItem.OnSelect();
		RootManager.instance.menuManagers[gameObject.scene.name] = this;
		canvas.worldCamera = RootManager.instance.mainCamera.camera;

		foreach( string scene in adjoinedScenes ) {
			SceneManager.LoadScene(scene, LoadSceneMode.Additive);
		}
	}

	private void OnDestroy()
	{
		RootManager.instance.menuManagers.Remove(gameObject.scene.name);
	}

	private void CollectMenuItems()
	{
		MenuItem[] items = GetComponentsInChildren<MenuItem>();
		int maxX = 0;
		int maxY = 0;
		for( int i = 0; i < items.Length; i++ ) {
			if( items[i].positionInMenu.x > maxX ) {
				maxX = items[i].positionInMenu.x;
			}
			if( items[i].positionInMenu.y > maxY ) {
				maxY = items[i].positionInMenu.y;
			}
		}

		menuItems = new MenuItem[maxX + 1, maxY + 1];
		for( int i = 0; i < items.Length; i++ ) {
			menuItems[items[i].positionInMenu.x, items[i].positionInMenu.y] = items[i];
		}
	}

	public delegate void SelectCallback();
	public virtual void OnSelect(SelectCallback callback = null)
	{
		if( canvas.renderMode == RenderMode.WorldSpace ) {
			RootManager.instance.PanCamera(transform.position, 0.5f);
		}
	}
	public virtual void OnDeselect(SelectCallback callback = null)
	{
		if( callback != null ) { callback.Invoke(); }
	}
 
	public virtual void OnNavigate(InputAction.CallbackContext context)
	{

		if( activeItem.OnNavigate(context) ) {
			return;
		}

		Vector2 value = context.ReadValue<Vector2>();
		if( context.performed ) {
			activeItem.OnDeselect();
			activeItemPos.x += (int)value.x;
			if( activeItemPos.x >= menuItems.GetLength(0) ) {
				activeItemPos.x = 0;
			}
			else if( activeItemPos.x < 0 ) {
				activeItemPos.x = menuItems.GetLength(0) - 1;
			}
			activeItemPos.y += (int)value.y;
			if( activeItemPos.y >= menuItems.GetLength(1) ) {
				activeItemPos.y = 0;
			}
			if( activeItemPos.y < 0 ) {
				activeItemPos.y = menuItems.GetLength(1) - 1;
			}
			if( activeItem == null ) {
				OnNavigate(context);
			}
			activeItem.OnSelect();
		}
	}
	public virtual void OnNext(InputAction.CallbackContext context)
	{
		if( activeItem.OnNext(context) ) {
			return;
		}

		if( context.performed ) {
			activeItem.OnDeselect();
			activeItemPos.y++;
			if( activeItemPos.y >= menuItems.GetLength(1) ) {
				activeItemPos.y = 0;
				activeItemPos.x++;
				if( activeItemPos.x >= menuItems.GetLength(0) ) {
					activeItemPos.x = 0;
				}
			}
			if( activeItem == null ) {
				OnNext(context);
			}
			activeItem.OnSelect();
		}
	}
	public virtual void OnSubmit(InputAction.CallbackContext context)
	{
		activeItem.OnSubmit(context);
	}
	public virtual void OnMenu(InputAction.CallbackContext context)
	{
		if( !activeItem.OnMenu(context) && closable ) {
			SceneManager.UnloadSceneAsync(gameObject.scene);
		}
	}
}
