using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Projectile : MonoBehaviour
{
    public bool alive = true;
    public GameObject explosionPrefab;
    public float explosionRadius = 1f;
    public float expansionRate = 0.01f;
    public float explosionLife = 1f;

    LineRenderer lineRenderer;

    Vector3 velocity = new Vector3();
    float deathStamp = 0;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Initialize(Vector3 velocity, float deathStamp)
    {
        this.velocity = velocity;
        this.deathStamp = deathStamp;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if( Time.time >= deathStamp ) {
            Explode();
            Destroy(gameObject);
            return;
        }

        transform.Translate(velocity, Space.World);
        transform.LookAt(transform.position + velocity);

        float lifetime = deathStamp - Time.time;

        Vector3 lineEnd = transform.position + (velocity * (lifetime * 50));
        lineRenderer.SetPositions(new Vector3[] { transform.position, lineEnd });
    }

    public void Explode()
    {
        alive = false;
        GameObject explosionObject = Instantiate<GameObject>(explosionPrefab, transform.position, Quaternion.identity);
        Explosion explosion = explosionObject.GetComponent<Explosion>();
        if( explosion != null) {
            explosion.Initialize(explosionRadius, explosionLife, expansionRate);
        }
    }
}
