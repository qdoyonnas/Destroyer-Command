using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnchor : MonoBehaviour
{
    List<Transform> targets;

    public float speed = 1;

	private void Awake()
	{
        targets = new List<Transform>();
	}

    public void AddTarget(Transform target)
    {
        targets.Add(target);
        Ship ship = target.gameObject.GetComponent<Ship>();
        if( ship != null ) {
            targets.Add(ship.GetAimer().transform);
        }
    }
    public void RemoveTarget(Transform target)
    {
        targets.Remove(target);
    }

	// Update is called once per frame
	void Update()
    {
        if( targets.Count == 0 ) { return; }

        float x = 0f, y = 0f, z = 0f;
        foreach( Transform t in targets ) {
            x += t.position.x;
            y += t.position.y;
            z += t.position.z;
        }

        Vector3 averagePosition = new Vector3(x / targets.Count, y / targets.Count, z / targets.Count);
        transform.position = averagePosition;
    }
}
