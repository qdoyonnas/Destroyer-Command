using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radius = 1f;
    public float lifeTime = 0f;
    public float deathStamp = 0f;
    public float expansionRate = 0.1f;

    new MeshRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();

        transform.localScale = Vector3.zero;
    }

    public void Initialize(float radius, float lifeTime, float expansionRate)
    {
        this.radius = radius;
        this.lifeTime = lifeTime;
        this.deathStamp = Time.time + lifeTime;
        this.expansionRate = expansionRate;

        if( this.expansionRate == -1 ) {
            transform.localScale = Vector3.one * radius;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( Time.time >= deathStamp ) {
            Destroy(gameObject);
            return;
        }

        Color color = renderer.material.color;
        color.a = (deathStamp - Time.time) / (lifeTime * 0.25f);
        renderer.material.color = color;

        if( transform.localScale.magnitude < radius ) {
            transform.localScale += Vector3.one * Mathf.Min(expansionRate, 1 - transform.localScale.magnitude / radius);
        }
    }
}
