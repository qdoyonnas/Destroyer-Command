using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sense : MonoBehaviour
{
	public enum SenseType {
		Ship,
		Hazard,
		Projectile
	}

	public SenseType[] types;
	public AIControls controls;
	public List<GameObject> objects;

	private void Start()
	{
		objects = new List<GameObject>();
	}

	private void Update()
	{
		for( int i = objects.Count-1; i >= 0; i-- ) {
            if( !objects[i] ) {
                objects.RemoveAt(i);
            }
        }
	}

	private void OnTriggerEnter(Collider other)
	{
		GameObject obj = null;
		if( types.Contains(SenseType.Hazard) ) {
				obj = other.GetComponentInParent<Hazard>() ? other.gameObject : null;
		}
		if( types.Contains(SenseType.Projectile) ) {
				obj = other.GetComponentInParent<Projectile>() ? other.gameObject : null;
		}
		if( types.Contains(SenseType.Ship) ) {
				obj = other.GetComponentInParent<Ship>() ? other.gameObject : null;
		}

		if( obj != null ) {
			objects.Add(obj);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		GameObject obj = null;
		if( types.Contains(SenseType.Hazard) ) {
				obj = other.GetComponentInParent<Hazard>() ? other.gameObject : null;
		}
		if( types.Contains(SenseType.Projectile) ) {
				obj = other.GetComponentInParent<Projectile>() ? other.gameObject : null;
		}
		if( types.Contains(SenseType.Ship) ) {
				obj = other.GetComponentInParent<Ship>() ? other.gameObject : null;
		}

		if( obj != null ) {
			objects.Remove(obj);
		}
	}
}
