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

    [HideInInspector] public Vector3 velocity = new Vector3();
    [HideInInspector] public float deathStamp = 0f;
    [HideInInspector] public float lifetime = 0f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Initialize(Vector3 velocity, float lifetime)
    {
        this.velocity = velocity;
        this.lifetime = lifetime;
        this.deathStamp = Time.time + lifetime;
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

        lifetime = deathStamp - Time.time;

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
