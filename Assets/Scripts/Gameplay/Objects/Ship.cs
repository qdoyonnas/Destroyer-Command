using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Ship : MonoBehaviour
{
    bool _disabled = true;
    public bool disabled {
        get {
            return _disabled;
        }
        set {
            SetDisabled(value);
        }
    }

    [Header("Movement")]
    public float minSpeed = 0.05f;
    public float maxSpeed = 1f;
    public float acceleration = 0.05f;
    public float turnRate = Mathf.PI * 0.25f;

    [Header("Aimer")]
    public float aimRange = 5f;
    public float aimDeceleration = 1f;
    public float aimDeadZone = 0.005f;

    [Header("Weapons")]
    public int activeWeapon = 0;
    public Weapon[] weapons;

    [Header("Explosion")]
    public bool alive = true;
    public GameObject explosionPrefab;
    public float explosionRadius = 70f;
    public float explosionLife = 5f;
    public float expansionRate = 0.2f;

    public Dictionary<string, float> inputs;

    private new CapsuleCollider collider;
    float speed = 0;

    public Controls controls;
    Aimer aimer;
    LineRenderer lineRenderer;

    public void Initialize()
    {
        lineRenderer = GetComponent<LineRenderer>();

        speed = minSpeed;
        collider = GetComponent<CapsuleCollider>();

        GameObject aimPrefab = Resources.Load<GameObject>("Prefabs/Aim");
        GameObject aimerObject = Instantiate( aimPrefab, transform.position + (transform.forward * 2), Quaternion.identity );
        aimerObject.name = gameObject.name + "_aim";
        aimer = aimerObject.GetComponent<Aimer>();
        aimer.Initialize(this, lineRenderer);

        for( int i = 0; i < weapons.Length; i++ ) {
            weapons[i] = Instantiate<ScriptableObject>(weapons[i]) as Weapon;
        }

        inputs = new Dictionary<string, float>();
        inputs["forward"] = 0;
        inputs["turn"] = 0;
        inputs["aimVertical"] = 0;
        inputs["aimHorizontal"] = 0;
        inputs["centerAim"] = 0;
        inputs["mainFire"] = 0;
        inputs["fireLock"] = 0;
    }

    public void SetHullColor(Color color)
    {
        transform.Find("Body").GetComponent<MeshRenderer>().material.color = color;
        transform.Find("Nose").GetComponent<MeshRenderer>().material.color = color;
    }
    public void SetTrailColor(Gradient color)
    {
        transform.Find("Trail").GetComponent<TrailRenderer>().colorGradient = color;
    }

    void FixedUpdate()
    {
        if( !disabled ) { HandleInputs(); }

        Vector3 oldPosition = transform.position;
        transform.Translate(transform.forward * speed, Space.World);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 positionDelta = transform.position - oldPosition;

        aimer.UpdateAimer(positionDelta);

        foreach( Weapon weapon in weapons ) {
            weapon.Update();
        }
    }

    public Aimer GetAimer()
    {
        return aimer;
    }
    public float GetSpeed()
    {
        return speed;
    }
    public Team GetTeam()
    {
        return controls.team;
    }
    public void SetSpeed(float value)
    {
        speed = value;
    }
    public Weapon GetActiveWeapon()
    {
        return weapons[activeWeapon];
    }

    void HandleInputs()
    {
        HandleMovement();
        HandleAimInput();
        HandleWeapons();
    }
    void HandleMovement()
    {
        if( inputs["forward"] != 0 ) {
            speed += inputs["forward"] * acceleration;
            if( speed < minSpeed ) {
                speed = minSpeed;
            } else if( speed > maxSpeed ) {
                speed = maxSpeed;
            }
        }

        if( inputs["turn"] != 0 ) {
            float turnRateModifier = turnRate - (turnRate * (speed/maxSpeed));
            float modifiedTurnRate = turnRate + turnRateModifier;
            transform.Rotate(0f, modifiedTurnRate * inputs["turn"], 0f, Space.World);
        }
    }
    void HandleAimInput()
    {
        aimer.HandleInput(inputs);
    }
    void HandleWeapons()
    {
        if( inputs["mainFire"] > 0 ) {
            GetActiveWeapon().Shoot(this, aimer.transform, inputs["fireLock"]);
            inputs["fireLock"] = 1;
        } else {
            inputs["fireLock"] = 0;
        }
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

    public void SetCollidable(bool state)
    {
        collider.enabled = state;
    }
    public void SetDisabled(bool state)
    {
        _disabled = state;
        if( controls is PlayerControls ) { aimer.SetVisible(!state); }
        aimer.transform.position = transform.position;
    }
}
