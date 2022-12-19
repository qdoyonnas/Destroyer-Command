using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapon: ScriptableObject
{
	public GameObject projectile;
	
	public int maxCharges = 1;
	public int charges = 1;
	public float chargeTime = 1f;
	private float chargeStamp = 0f;

	public float shotDelay = -1f; // -1 is click speed
	private float shotStamp = 0f;

	public float shotVelocity = 1f;

	public float startAngle = 0f;
	public float endAngle = 165f;

	public void Update()
	{
		if( charges < maxCharges ) {
			if( Time.time > chargeStamp ) {
				charges++;
				chargeStamp = Time.time + chargeTime;
			}
		}
	}

	public void Shoot(Ship launcher, Transform aimer, float fireLock)
	{
		if( !InAngle(launcher, aimer) ) { return; }
		if( Time.time < shotStamp
			|| (shotDelay == -1 && fireLock > 0) ) {
			return;
		}
		if( charges > 0 ) {
			charges -= 1;
			chargeStamp = Time.time + chargeTime;
		} else {
			return;
		}

		shotStamp = Time.time + shotDelay;

		GameObject newProjectile = Instantiate<GameObject>(projectile, launcher.transform.position, Quaternion.identity);
		Projectile scriptProjectile = newProjectile.GetComponent<Projectile>();
		
		if( scriptProjectile == null) { 
			Destroy(newProjectile);
			return;
		}

		
		Vector3 delta = aimer.position - launcher.transform.position;
		delta.y = 0;
		float lifetime = (delta.magnitude / shotVelocity) / 50;

		Vector3 velocity = delta.normalized * shotVelocity;
		scriptProjectile.Initialize(velocity, lifetime);
	}

	public bool InAngle(Ship launcher, Transform aimer)
	{
		Vector3 delta = aimer.position - launcher.transform.position;
		float angle = Vector3.Angle(launcher.transform.forward, delta);

		return startAngle <= angle && angle <= endAngle;
	}
}
