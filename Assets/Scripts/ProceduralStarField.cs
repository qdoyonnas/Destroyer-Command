using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralStarField : MonoBehaviour
{
    public GameObject starPrefab;

    private Transform starParent;

    public void Start()
    {
        starParent = new GameObject("Stars").transform;
    }

    public struct Star
    {
        public readonly Vector3 position;
        public readonly float angle;
        public readonly Color color;

        public Star( Vector3 position, float angle, Color color )
        {
            this.position = position;
            this.angle = angle;
            this.color = color;
        }
    }

    public Star[] NearStars( Vector3 position, int distance )
    {
        List<Star> stars = new List<Star>();

        for( int x = -distance; x < distance; x++ ) {
            for( int y = -distance; y < distance; y++ ) {
                for( int z = -distance; z < distance; z++ ) {
                    Vector3 starPos = new Vector3(x, y, z) + position;
                    if( StarExists(starPos) ) {
                        stars.Add(new Star(starPos, 0, Color.white));
                    }
                }
            }
        }

        return stars.ToArray();
    }

    private bool StarExists( Vector3 position )
    {
        int positionValue = GetPositionValue(position);

        Random.InitState(positionValue);// + GameManager.instance.worldSeed);

        float value = Random.value;
        Debug.Log(position);
        Debug.Log($"Position Value: {positionValue}");
        Debug.Log($"Random Value: {value}");
        Debug.Log("--------------------------");
        return value < 0.02f;
    }

    private int GetPositionValue( Vector3 position )
    {
        return ( ((int)position.x) << 10 | ((int)position.y) ) << 10 | ((int)position.z);
    }

    public void Update()
    {
        if( starPrefab == null ) { return; }

        if( Input.GetKeyDown(KeyCode.Space) ) {
            Destroy(starParent.gameObject);
            starParent = new GameObject("Stars").transform;

            Star[] stars = NearStars(transform.position, 10);
            foreach( Star star in stars ) {
                GameObject starObject = Instantiate(starPrefab, star.position, Quaternion.Euler(90, star.angle, 0), starParent);
            }
        }
    }
}
