using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeFacility : Facility
{
	public bool performSpawn = true;
    public GameObject shipPrefab;
	public Transform spawnPosition;
	public Transform spawnExit;
	public int shipCount = 20;
	public float spawnTime = 5f;

	public int nextPlayerSpawn = -1;

	private float spawnTimestamp = 0f;
	private Hangar hangar;

	private Ship spawnedShip;
	private float undockRate = 0f;

	private void Start()
	{
		hangar = GetComponentInChildren<Hangar>();
		hangar.teamsList = new Team[] { team };
		hangar.teamsWhitelist = false;

		spawnTimestamp = Time.time + 0.5f;
		undockRate = Vector3.Distance(spawnPosition.position, spawnExit.position) / 1f;
	}

	private void FixedUpdate()
	{
		if( performSpawn && Time.time > spawnTimestamp ) {
			SpawnShip(shipPrefab, spawnPosition.position, spawnExit.eulerAngles.y, nextPlayerSpawn, team);
			spawnTimestamp = Time.time + spawnTime;
		}
		if( spawnedShip && TweenObject(spawnedShip.gameObject, spawnExit.position, undockRate) ) {
			spawnedShip.SetDisabled(false);
			spawnedShip.SetCollidable(true);
			spawnedShip.SetSpeed(spawnedShip.maxSpeed / 2f);
			spawnedShip = null;
		}
	}

	private Ship SpawnShip(GameObject prefab, Vector3 position, float angle, int playerNumber, Team team)
    {
		if( spawnedShip ) { return null; }

        GameObject shipObject = Instantiate<GameObject>(prefab, position, Quaternion.Euler(0, angle, 0));
        Ship ship = shipObject.GetComponent<Ship>();
        ship.Initialize();
        if( playerNumber == -1 ) {
            AIControls ai = shipObject.AddComponent<AIControls>();
            ai.team = team;
            shipObject.name = "AI_" + shipObject.name;
        } else {
            PlayerControls player = shipObject.AddComponent<PlayerControls>();
            player.playerNumber = playerNumber;
            player.team = team;
            GameManager.instance.players.Add(player);
            shipObject.name = "Player_" + shipObject.name;
			GameManager.instance.mainCamera.AddTarget(shipObject.transform);
        }

		nextPlayerSpawn = -1;

		ship.SetDisabled(true);
		ship.SetCollidable(false);
		spawnedShip = ship;
        return ship;
    }

	private bool TweenObject(GameObject obj, Vector3 target, float rate)
	{
		float distance = Vector3.Distance(obj.transform.position, target);
		if( distance > 0.4f ) {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, rate * Time.deltaTime);
			
			return false;
        }

		return true;
	}

	public override void Capture(Team team)
	{
		GameManager.instance.DisplayGameOver(team);
	}
}
