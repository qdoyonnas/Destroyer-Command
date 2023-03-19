using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RootManager : MonoBehaviour
{
    #region Singleton

    private static RootManager _instance;
    public static RootManager instance {
        get {
            return _instance;
        }
    }

    #endregion

    #region Fields

    public CameraAnchor mainCamera;

    public string firstScene = "MainMenuScene";
    public Dictionary<string, MenuManager> menuManagers = new Dictionary<string, MenuManager>();
    [HideInInspector] private string activeManagerScene;
    public MenuManager activeManager {
        get {
            if( menuManagers.ContainsKey(activeManagerScene) ) {
                return menuManagers[activeManagerScene];
            } else {
                return null;
            }
        }
    }

    private PlayerInput _playerInput;
    public PlayerInput playerInput {
        get {
            return _playerInput;
        }
    }

    // TODO: Panning should be logic of the camera anchor
    private Vector3 panPosition = new Vector3();
    private float panRate = 0f;

    #endregion

    void Start()
    {
        if( _instance == null ) {
            _instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        _playerInput = GetComponent<PlayerInput>();

        mainCamera = GameObject.Find("CameraAnchor").GetComponent<CameraAnchor>();

        SceneManager.LoadScene(firstScene, LoadSceneMode.Additive);
        activeManagerScene = firstScene;
    }

	private void FixedUpdate()
	{
        if( mainCamera.transform.position != panPosition ) {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, panPosition, panRate * Time.deltaTime);
        }
	}

    public void CloseAllScenes()
    {
        SceneManager.SetActiveScene(gameObject.scene);
        mainCamera.targets = new List<Transform>();

        for( int i = 1; i < SceneManager.sceneCount; i++) {
            Scene scene = SceneManager.GetSceneAt(i);
            if( scene != gameObject.scene ) {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        mainCamera.transform.position = Vector3.zero;
        Time.timeScale = 1f;
    }

    public void SwapMenu(string name)
    {
        if( activeManager ) {
            activeManager.OnDeselect(() => {
                activeManagerScene = name;
                if( activeManager ) { activeManager.OnSelect(); }
            });
        } else {
            activeManagerScene = name;
            if( activeManager ) { activeManager.OnSelect(); }
        }
    }

    public void PanCamera(Vector3 position, float time)
    {
        panPosition = position;
        float distance = Vector3.Distance(mainCamera.transform.position, position);
        panRate = distance / time;
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        if( activeManager ) { activeManager.OnNavigate(context); }
    }
    public void OnNext(InputAction.CallbackContext context)
    {
        if( activeManager ) { activeManager.OnNext(context); }
    }
    public void OnSubmit(InputAction.CallbackContext context)
    {
        if( activeManager ) { activeManager.OnSubmit(context); }
    }
    public void OnMenu(InputAction.CallbackContext context)
    {
        if( activeManager ) { activeManager.OnMenu(context); }
    }
}
