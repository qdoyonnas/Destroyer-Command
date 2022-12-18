using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// XXX: Make procedural
public class StarField : MonoBehaviour
{
    public GameObject starPrefab;

    public Transform cameraAnchor;

    public int maxStarCount = 100;
    public float[] sizeRange = new float[] { 0.2f, 0.5f };
    public Vector3 minZone = new Vector3(-20, 0, -20);
    public Vector3 maxZone = new Vector3(20, 0, 20);

    public float parallaxFactor = 0.05f;

    Vector3 lastCameraPosition = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        if( cameraAnchor == null ) { return; }

        for( int i = 0; i < maxStarCount; i++ ) {
            GenerateStar(minZone, maxZone);
        }

        lastCameraPosition = cameraAnchor.position;
    }

    SpriteRenderer GenerateStar(Vector3 min, Vector3 max)
    {
        Vector3 position = new Vector3( Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z) );

        SpriteRenderer star = Instantiate(starPrefab, position + transform.position, Quaternion.identity, transform).GetComponent<SpriteRenderer>();
        RandomizeStar(star);

        return star;
    }

    void RandomizeStar(SpriteRenderer star)
    {
        float scale = Random.Range(sizeRange[0], sizeRange[1]);
        Quaternion rotation = Quaternion.Euler(90, Random.value * 360, 0);

        star.transform.localRotation = rotation;
        star.transform.localScale = Vector3.one * scale;
        star.color = Random.ColorHSV();
    }

    // Update is called once per frame
    void Update()
    {
        if( cameraAnchor.position != lastCameraPosition ) {
            Vector3 translation = cameraAnchor.position - lastCameraPosition;
            lastCameraPosition = cameraAnchor.position;

            for( int i = transform.childCount - 1; i >= 0; i-- ) {
                Transform star = transform.GetChild(i);
                star.position -= (translation * star.localScale.magnitude) * parallaxFactor;

                if( star.localPosition.magnitude > maxZone.x ) {
                    star.localPosition = new Vector3(-star.localPosition.x, star.localPosition.y, -star.localPosition.z);
                    RandomizeStar( star.GetComponent<SpriteRenderer>() );
                }
            }
        }
    }
}
