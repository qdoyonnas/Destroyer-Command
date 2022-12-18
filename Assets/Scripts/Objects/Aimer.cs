using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour
{
    Ship ship;
    Vector3 aimVelocity = new Vector3();

    GameObject[] charges = new GameObject[0];

    LineRenderer lineRenderer;

    GameObject chargePrefab;
    Material enabledCharge;
    Material disabledCharge;

    Material enabledTexture;
    Material disabledTexture;

    MeshRenderer crossRenderer;
    new MeshRenderer renderer;

    bool wasInAngle = false;

	public void Initialize(Ship parent, LineRenderer line)
    {
        ship = parent;
        lineRenderer = line;

        chargePrefab = Resources.Load<GameObject>("Prefabs/Charge");
        enabledCharge = Resources.Load<Material>("Materials/Yellow");
        disabledCharge = Resources.Load<Material>("Materials/Gray 2");

        enabledTexture = Resources.Load<Material>("Materials/Ring");
        disabledTexture = Resources.Load<Material>("Materials/Ring Gray");

        crossRenderer = transform.Find("Cross").GetComponent<MeshRenderer>();
        renderer = GetComponent<MeshRenderer>();

        charges = new GameObject[0];
    }

    public void HandleInput(Dictionary<string, float> inputs)
    {
        Vector3 aimDelta = new Vector3(inputs["aimHorizontal"], 0f, inputs["aimVertical"]);
        aimVelocity = aimDelta;

        if ( inputs["centerAim"] != 0 ) {
            transform.position = ship.transform.position;
        }
    }

    public void UpdateAimer(Vector3 positionDelta)
    {
        transform.Translate(positionDelta);
        transform.Translate(aimVelocity);

        if (Vector3.Distance(transform.position, ship.transform.position) > ship.aimRange)
        {
            Vector3 unitVector = (transform.position - ship.transform.position).normalized;
            transform.position = ship.transform.position + (unitVector * ship.aimRange);
        }

        Vector3 offset = transform.position - ship.transform.position;
        lineRenderer.SetPosition(0, ship.transform.position);
        lineRenderer.SetPosition(1, ship.transform.position + (offset * 0.5f));
        lineRenderer.SetPosition(2, transform.position);

        UpdateWeaponDisplay();
    }

    void UpdateWeaponDisplay()
    {
        UpdateCharges();
        UpdateAngle();
    }

    void UpdateCharges()
    {
        int weaponMaxCharges = ship.GetActiveWeapon().maxCharges;
        int weaponCharges = ship.GetActiveWeapon().charges;
        if( charges.Length != weaponMaxCharges ) {
            for( int i = charges.Length; i > 0; i-- ) {
                Destroy(charges[i]);
            }
            charges = new GameObject[weaponMaxCharges];
            float angle = 0f;
            float angleSpacing = (2*Mathf.PI) / weaponMaxCharges;
            for( int i = 0; i < charges.Length; i++ ) {
                Vector3 position = new Vector3(transform.position.x + Mathf.Cos(angle) * 3, 0, transform.position.z + Mathf.Sin(angle) * 3);
                charges[i] = Instantiate<GameObject>(chargePrefab, position, Quaternion.identity, transform);
                charges[i].GetComponent<MeshRenderer>().enabled = renderer.enabled;
                angle += angleSpacing;
            }
        }

        for( int i = 0; i < charges.Length; i++ ) {
            charges[i].SetActive(i < weaponCharges);
        }
    }
    void UpdateAngle()
    {
        bool inAngle = ship.GetActiveWeapon().InAngle(ship, transform);
        if( inAngle == wasInAngle ) { return; }

        if( inAngle ) {
            crossRenderer.enabled = false;
            renderer.material = enabledTexture;

            for( int i = 0; i < charges.Length; i++ ) {
                charges[i].GetComponent<MeshRenderer>().material = enabledCharge;
            }
        } else {
            crossRenderer.enabled = renderer.enabled;
            renderer.material = disabledTexture;

            for( int i = 0; i < charges.Length; i++ ) {
                charges[i].GetComponent<MeshRenderer>().material = disabledCharge;
            }
        }

        wasInAngle = inAngle;
    }

    public void SetVisible(bool state)
    {
        lineRenderer.enabled = state;
        renderer.enabled = state;
        crossRenderer.enabled = state;
        for( int i = 0; i < charges.Length; i++ ) {
            charges[i].GetComponent<MeshRenderer>().enabled = state;
        }
    }
}
