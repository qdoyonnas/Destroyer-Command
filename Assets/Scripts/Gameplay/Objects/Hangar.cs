using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hangar : MonoBehaviour
{
	public Team[] teamsList;
	public bool teamsWhitelist = false;

	public Transform closedPosition;
	public Transform hangarPosition;
	public Transform hangarExit;
	public float holdTime = 2f;
	public float transitionTime = 1f;

	public float defaultLockTime = 30f;

	public Facility facility;
	 // TODO: This doesn't work yet. Needs to be separated into Entrance, Hangar, and exit objects
	[HideInInspector] public List<Ship> dockedShips = new List<Ship>();

	Vector3 shipTarget;
	float shipRate;
	Vector3 hangarTarget;
	float hangarRate = 5f;

	float exitTimeStamp = 0f;
	int phase = 0;

	float lockedTimestamp = 0f;

	Vector3 openPosition;

	private void Start()
	{
		openPosition = transform.position;
		hangarTarget = openPosition;
	}

	private void FixedUpdate()
	{
		foreach( Ship ship in dockedShips ) {
			HandleShip(ship);
		}
		TweenObject(gameObject, hangarTarget, hangarRate);
	}

	private void HandleShip(Ship dockedShip)
	{
		switch(phase) {
			case 1:
				if( TweenObject(dockedShip.gameObject, shipTarget, shipRate) ) {
					exitTimeStamp = Time.time + holdTime;
					phase = 2;
				}
				break;
			case 2:
				if( Time.time >= exitTimeStamp ) {
					if( facility ) { facility.Capture(dockedShip.GetTeam()); }
					shipTarget = hangarExit.position;
					Vector3 angles = dockedShip.transform.eulerAngles;
					dockedShip.transform.eulerAngles = new Vector3(angles.x, hangarExit.eulerAngles.y, angles.z);
					shipRate = Vector3.Distance(dockedShip.transform.position, hangarExit.position) / transitionTime;
					phase = 3;
				}
				break;
			case 3:
				if( TweenObject(dockedShip.gameObject, shipTarget, shipRate) ) {
					dockedShip.SetDisabled(false);
					dockedShip.SetCollidable(true);
					dockedShip.SetSpeed(dockedShip.minSpeed);
					dockedShip = null;
					hangarTarget = openPosition;
					phase = 0;
				}
				break;
		}
	}

	private bool TweenObject(GameObject obj, Vector3 target, float rate)
	{
		if( obj.transform.position != target ) {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, rate * Time.deltaTime);
			
			return false;
        }

		return true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if( Time.time > lockedTimestamp ) {
			Ship ship = other.gameObject.GetComponent<Ship>();
			if( ship && (Array.IndexOf(teamsList, ship.GetTeam()) != -1) == teamsWhitelist) {
				DockShip(ship, defaultLockTime);
			}
		}
	}

	private void DockShip(Ship ship, float lockTime = -1f)
	{
		dockedShips.Add(ship);
		ship.SetCollidable(false);
		ship.SetDisabled(true);
		ship.SetSpeed(0f);
		hangarTarget = closedPosition.position;
		shipTarget = hangarPosition.position;
		shipRate = Vector3.Distance(ship.transform.position, hangarPosition.position) / transitionTime;
		phase = 1;

		lockedTimestamp = Time.time + lockTime;
	}
}
