using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Team[] teams;

    public List<PlayerControls> players;

    public GameObject asteroidPrefab;
    public GameObject shipPrefab;
    public int worldSeed = 0;

    public CameraAnchor mainCamera;

    #endregion

    void Start()
    {
        if( _instance == null ) {
            _instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        mainCamera = GameObject.Find("CameraAnchor").GetComponent<CameraAnchor>();

        players = new List<PlayerControls>();
        float spawnAngle = Random.Range(0, 360);
        Vector3 playerPosition = new Vector3(
            transform.position.x + Random.Range(-200, 200), 
            0, 
            transform.position.z + Random.Range(-200, 200)
        );
        Ship ship = SpawnShip(shipPrefab, playerPosition, spawnAngle, 0, 0);
        mainCamera.AddTarget(ship.transform);


        for( int i = 0; i < 50; i++ ) {
            Vector3 spawnPosition = new Vector3(Random.Range(-1000, 1000), 0f, Random.Range(-1000, 1000));
            GameObject asteroidObject = Instantiate<GameObject>(asteroidPrefab, spawnPosition, Quaternion.identity);
            Asteroid asteroid = asteroidObject.GetComponent<Asteroid>();
            asteroid.Initialize();
        }
    }

	void Update()
	{
		if( Input.GetKeyDown(KeyCode.Backspace) ) {
            float spawnAngle = Random.Range(0, 360);
            Vector3 spawnPosition = new Vector3(
                players[0].transform.position.x + Random.Range(-200, 200), 
                0, 
                players[0].transform.position.z + Random.Range(-200, 200)
            );
            SpawnShip(shipPrefab, spawnPosition, spawnAngle, -1, 1);
        }

        if( Input.GetKeyDown(KeyCode.Return) && players.Count == 0 ) {
            float spawnAngle = Random.Range(0, 360);
            Vector3 spawnPosition = new Vector3(
                transform.position.x + Random.Range(-200, 200), 
                0, 
                transform.position.z + Random.Range(-200, 200)
            );
            Ship ship = SpawnShip(shipPrefab, spawnPosition, spawnAngle, 0, 0);
            mainCamera.AddTarget(ship.transform);
        }
	}

    public Ship SpawnShip(GameObject prefab, Vector3 position, float angle, int playerNumber, int team)
    {
        GameObject shipObject = Instantiate<GameObject>(prefab, position, Quaternion.Euler(0, angle, 0));
        Ship ship = shipObject.GetComponent<Ship>();
        if( playerNumber == -1 ) {
            AIControls ai = shipObject.AddComponent<AIControls>();
            ai.team = teams[team];
        } else {
            PlayerControls player = shipObject.AddComponent<PlayerControls>();
            player.playerNumber = playerNumber;
            player.team = teams[team];
            players.Add(player);
        }

        ship.Initialize();
        return ship;
    }

	public void PlayerDeath(PlayerControls player)
    {
        mainCamera.RemoveTarget(player.transform);
        mainCamera.RemoveTarget(player.GetShip().GetAimer().transform);
        players.Remove(player);
    }
}
