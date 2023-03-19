using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;
    public static GameManager instance {
        get {
            return _instance;
        }
    }

    #endregion

    #region Fields

    public HomeFacility playerFacility;
    public string pauseMenuName = "PauseMenuScene";
    public string gameOverSceneName = "GameOverScene";
    public bool gameOver = false;

    public Team[] teams;

    public List<PlayerControls> players;

    public GameObject asteroidPrefab;
    public int worldSeed = 0;

    [HideInInspector] public CameraAnchor mainCamera;
    [HideInInspector] public PlayerInput playerInput;

    #endregion

    void Start()
    {
        if( _instance == null ) {
            _instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        Init();

        players = new List<PlayerControls>();
        float spawnAngle = Random.Range(0, 360);
        Vector3 playerPosition = new Vector3(
            transform.position.x + Random.Range(-200, 200), 
            0, 
            transform.position.z + Random.Range(-200, 200)
        );

        SpawnAsteroids();
    }

	private void Update()
	{
		if( players.Count < 1 ) {
            if( playerFacility ) { 
                playerFacility.nextPlayerSpawn = 0;
            }
        }
	}

	private void OnDestroy()
	{
		playerInput.currentActionMap.FindAction("Next").performed -= OnNext;
        playerInput.currentActionMap.FindAction("Submit").performed -= OnSubmit;
        playerInput.currentActionMap.FindAction("Menu").performed -= OnMenu;
	}

	private void Init()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.SetActiveScene(gameObject.scene);

        Random.InitState(worldSeed);

        mainCamera = RootManager.instance.mainCamera;

        playerInput = RootManager.instance.playerInput;
        playerInput.currentActionMap.FindAction("Next").performed += OnNext;
        playerInput.currentActionMap.FindAction("Submit").performed += OnSubmit;
        playerInput.currentActionMap.FindAction("Menu").performed += OnMenu;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if( scene.name == pauseMenuName ) {
            Time.timeScale = 1f;
        }
    }

    private void SpawnAsteroids()
    {
        for( int i = 0; i < 200; i++ ) {
            Vector3 spawnPosition = new Vector3(Random.Range(-1500, 1500), 0f, Random.Range(-1500, 1500));
            GameObject asteroidObject = Instantiate<GameObject>(asteroidPrefab, spawnPosition, Quaternion.identity);
            Asteroid asteroid = asteroidObject.GetComponent<Asteroid>();
            asteroid.Initialize();
        }
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
    }
    public void OnNext(InputAction.CallbackContext context)
    {
    }
    public void OnMenu(InputAction.CallbackContext context)
    {
        Scene pauseScene = SceneManager.GetSceneByName(pauseMenuName);
        if( !pauseScene.isLoaded ) {
            SceneManager.LoadScene(pauseMenuName, LoadSceneMode.Additive);
            RootManager.instance.SwapMenu(pauseMenuName);
            if( players.Count <= 1 ) {
                Time.timeScale = 0f;
            }
        }
    }

    public void DisplayGameOver(Team team)
    {
        gameOver = true;

        Scene pauseScene = SceneManager.GetSceneByName(pauseMenuName);
        if( pauseScene.isLoaded ) {
            SceneManager.UnloadSceneAsync(pauseScene);
        }
        SceneManager.LoadScene(gameOverSceneName, LoadSceneMode.Additive);
        RootManager.instance.SwapMenu(gameOverSceneName);
        Time.timeScale = 0f;
    }

	public void PlayerDeath(PlayerControls player)
    {
        mainCamera.RemoveTarget(player.transform);
        mainCamera.RemoveTarget(player.GetShip().GetAimer().transform);
        players.Remove(player);
    }
}
