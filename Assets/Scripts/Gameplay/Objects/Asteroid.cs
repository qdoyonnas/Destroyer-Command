using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float minSize = 15f;
    public float maxSize = 50f;

    public int minSubSections = 3;
    public int maxSubSections = 10;

    private GameObject asteroidPrefab1;
    private GameObject asteroidPrefab2;

	public void Awake()
	{
		asteroidPrefab1 = Resources.Load<GameObject>("Prefabs/Asteroid1");
        asteroidPrefab2 = Resources.Load<GameObject>("Prefabs/Asteroid2");
	}

	public void Initialize()
    {
        float size = Random.Range(minSize, maxSize);
        transform.localScale = Vector3.one * size;
        transform.eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        if( maxSubSections > 0 ) {
            int subSections = Random.Range(minSubSections, maxSubSections);
            for( int i = 0; i < subSections; i++ ) {
                float spawnAngle = Random.Range(0, 2*Mathf.PI);
                float spawnDistance = Random.Range(size * 0.1f, size * 0.6f);
                Vector3 spawnOffset = new Vector3(Mathf.Cos(spawnAngle), 0, Mathf.Sin(spawnAngle)) * spawnDistance;
                Vector3 spawnPosition = transform.position + spawnOffset;
                GameObject asteroidObject;
                if( Random.value < 0.5f ) {
                    asteroidObject = Instantiate<GameObject>(asteroidPrefab2, spawnPosition, Quaternion.identity, transform);
                } else {
                    asteroidObject = Instantiate<GameObject>(asteroidPrefab2, spawnPosition, Quaternion.identity, transform);
                }
            
                Asteroid asteroid = asteroidObject.GetComponent<Asteroid>();
                asteroid.Initialize(0.8f, 1.2f, 0, Mathf.Max(0, maxSubSections - 4));
            }
        }
    }
    public void Initialize(float minSize, float maxSize, int minSubSections, int maxSubSections)
    {
        this.minSize = minSize;
        this.maxSize = maxSize;
        this.minSubSections = minSubSections;
        this.maxSubSections = maxSubSections;
        Destroy(gameObject.GetComponent<Rigidbody>());
        Initialize();
    }
}
