using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlightInput
{
    public float pitch = 0;
    public float yaw = 0;
    public float roll = 0;

    public bool Reheat = false;
    public float Throttle = 1;
}



public class sufa : MonoBehaviour
{
    [Header("Unity Properties")]
    public Camera hudCam;
    public Camera backCam;
    public GameObject hud;
    public bool cam;
    public bool useFixedUpdates = true;
    public FlightInput flightInput = new FlightInput();
    public bool isPlayer = true;

    [Header("Motion")]
    public float startSpeed;
    public Vector3 velocity { get; private set; } = Vector3.zero;
    public Vector3 acceleration { get; private set; } = Vector3.zero;
    public Vector3 VelocityDirection { get; private set; } = Vector3.zero;

    public float Speed;


    [Header("Drag")]
    public float Drag = 1.5f;
    public float InducedDrag = .75f;
    public float DynamicStallSpeed  = 77f;
    public float StallAOA = 15f;
    public float Responsiveness = 3f;

    [Header("Thrust to Weight")]
    [Tooltip("Kilograms")]
    public float Mass = 14500f;
    [Tooltip("Newtons")]
    public float MilThrust = 79000f;
    [Tooltip("Newtons")]
    public float ReheatThrust = 129000f;

    [Header("Rotation")]
    public float maxRollSpeed = 120f;
    public float rollAcc = 6f;
    private float roll = 0;

   [Space]
    public float maxPitchSpeed = 20f;
    public float PitchAcc = 6f;
    private float pitch = 0;

    [Space]
    public float maxYawSpeed = 10f;
    public float YawAcc = 5f;
    private float yaw = 0;

    [Space]
    public float g;
    public float gSmoothed;
    public float gLimit = 9f;
    public float gNegLimit = 2f;

    [Header("HUD INFO")]
    public float altitude;
    public float rAltitude;
    public float engThrottle;
    public bool ab;
    public ParticleSystem flames;


    public static sufa Player { get; private set; } = null;
    public float Scale = 1f;


    void Awake()
    {
        Application.targetFrameRate = 70; // Sets target FPS
        SetCam(); // Sets active camera + hud

        if (isPlayer)//Sets global player
            Player = this;

        //Initial stats
        startSpeed = 200f;//mps
        Speed = startSpeed;
        altitude = transform.position.y;
        velocity = transform.forward * startSpeed;
        VelocityDirection = velocity.normalized;
        flames.Stop(); // AB off
    }




    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cam = !cam;
            SetCam();
        }
        getPlayerInput();
        RunModel(Time.deltaTime);
        
    }

    private void SetCam()
    {
        backCam.gameObject.SetActive(cam);
        hudCam.gameObject.SetActive(!cam);
        hud.SetActive(!cam);
    }

    private void RunModel(float deltaTime)
    {
        RunFlightModelRotations(deltaTime);
        RunFlightModelLinear(deltaTime);

        altitude += velocity.y * deltaTime;
        rAltitude = altitude - Terrain.activeTerrain.SampleHeight(transform.position) * Scale;
        if(-transform.up.y>-0.5)
        {
            rAltitude = -999999f;
        }       
    }

    private void RunFlightModelLinear(float deltaTime)
    {
        // Gravity
        Vector3 gravityForce = Physics.gravity * Mass;

        // Thrust
        float thrust = flightInput.Reheat ? ReheatThrust : flightInput.Throttle * MilThrust;
        Vector3 thrustForce = transform.forward * thrust;

        // Drag holds the plane back the faster it goes, until it eventually reaches an equilibrium
        // between the drag force and thrust at the plane's top speed.
        float linearDrag = Mathf.Pow(Speed, 2f) * Drag;
        float totalDrag = linearDrag;

        Vector3 dragForce = -transform.forward * totalDrag;

        // Induced drag decreases speed when turning. The higher the angle of attack, the more drag.
        var inducedAOA = Vector3.Angle(transform.forward, velocity.normalized);
        Vector3 inducedDragForce = -transform.forward * Mathf.Pow(Speed, 2f) * InducedDrag * inducedAOA;

        // Consider the forces only as they affect forward speed as a simplification of physics.
        var acceleration = (gravityForce + thrustForce + dragForce + inducedDragForce) / Mass;
        var forwardAccel = Vector3.Dot(transform.forward, acceleration);
        Speed += forwardAccel * deltaTime;

        // Stalling will turn the velocity vector down towards the ground.
        var stallAOA = Maths.Remap(DynamicStallSpeed, DynamicStallSpeed * 1.5f, StallAOA, 0f, Speed);

        // The direction that the velocity vector would ideally face. This includes things that
        // affect it such as stalling, which lowers the velocity vector towards the ground.
        var targetVelocityVector = transform.forward;
        targetVelocityVector = Vector3.RotateTowards(targetVelocityVector, Vector3.down, stallAOA * Mathf.Deg2Rad, 0f);

        // Change the direction of the velocity smoothly so that some alpha gets generated.
        VelocityDirection = SmoothDamp.Move(
            VelocityDirection, targetVelocityVector,
            Responsiveness, deltaTime);

        velocity = VelocityDirection * Speed;
        transform.position += velocity * Scale * deltaTime;
    }





    private void throttle()
    {
        float targetThrottle;
        float throttleSpeed;
        if (Input.GetButton("Fire3"))
        {
            targetThrottle = 1f;
            throttleSpeed = .25f;
        }
        else if (Input.GetButton("Fire2"))
        {
            targetThrottle = 0f;
            throttleSpeed = .25f;
            flightInput.Reheat = false;
            flames.Stop();
        }
        else
        {
            targetThrottle = flightInput.Throttle;
            throttleSpeed = 0f;
        }

        flightInput.Throttle = Mathf.MoveTowards(
            flightInput.Throttle,
            targetThrottle,
            throttleSpeed * Time.deltaTime);

        if (Mathf.Approximately(flightInput.Throttle, 1f) && Input.GetButtonDown("Fire3"))
        {
            flightInput.Reheat = true;
            flames.Play();
        }

        engThrottle = flightInput.Throttle;
        ab = flightInput.Reheat;
    }

    

    private void RunFlightModelRotations(float deltaTime)
    {
        g = Maths.CalculatePitchG(transform, velocity, pitch);
        gSmoothed = SmoothDamp.Move(gSmoothed, g, 3f, deltaTime);

        // The stall speed affects low speed handling. The lower the stall speed, the more control
        // the plane has at low speeds. A high stall speed results in not only poor control at low
        // speed, but also requires more speed to generate the maximum turn rate.
        var controlAuthority = Mathf.InverseLerp(DynamicStallSpeed * .5f, DynamicStallSpeed * 2.5f, Speed); ;

        var glerp = 0f;
        if (g > 0)
            glerp = Mathf.InverseLerp(gLimit - 0.1f, gLimit + 0.9f, g);
        else
            glerp = Mathf.InverseLerp(-gNegLimit, -gNegLimit - 1f, g);

        var glimiter = Mathf.InverseLerp(0f, 1f, 1f - glerp);

        // For each axis, generate a rotation and then damp it to create smooth motion.
        var targetPitch = flightInput.pitch * maxPitchSpeed * glimiter * controlAuthority;
        pitch = SmoothDamp.Move(pitch, targetPitch, PitchAcc, deltaTime);
        var pitchRotation = Quaternion.AngleAxis(pitch * deltaTime, Vector3.right);

        var targetYaw = flightInput.yaw * maxYawSpeed * controlAuthority;
        yaw = SmoothDamp.Move(yaw, targetYaw, YawAcc, deltaTime);
        var yawRotation = Quaternion.AngleAxis(yaw * deltaTime, Vector3.up);

        var targetRoll = flightInput.roll * maxRollSpeed * controlAuthority;
        roll = SmoothDamp.Move(roll, targetRoll, rollAcc, deltaTime);
        var rollRotation = Quaternion.AngleAxis(-roll * deltaTime, Vector3.forward);

        transform.localRotation *= pitchRotation * rollRotation * yawRotation;

        // When stalling, the plane pitches down towards the ground.
        var stallRate = GetStallRate();
        if (stallRate > 0f)
        {
            // Generate stall rotation.
            var stallAxis = Vector3.Cross(transform.forward, Vector3.down);
            transform.rotation = Quaternion.AngleAxis(stallRate * deltaTime, stallAxis) * transform.rotation;
        }
    }

    private float GetStallRate()
    {
        // When stalling, the plane pitches down towards the ground.
        var stallRate = Maths.Remap(
            DynamicStallSpeed * .75f, DynamicStallSpeed * 1.25f,
            maxPitchSpeed, 0f,
            Speed);

        // Decrease stall turning power as the plane faces down.
        stallRate *= 1f - Vector3.Dot(transform.forward, Vector3.down);
        return stallRate;
    }


    private void getPlayerInput()
    {
        flightInput.roll = Input.GetAxis("Horizontal");
        flightInput.pitch = Input.GetAxis("Vertical");
        flightInput.yaw = Input.GetAxis("qe");
        throttle();
    }


}

public static class Maths
{
    /// <summary>
    /// Remaps the value <paramref name="value"/> from range X to range Y.
    /// </summary>
    public static float Remap(float xLow, float xHigh, float yLow, float yHigh, float value)
    {
        var lerp = Mathf.InverseLerp(xLow, xHigh, value);
        return Mathf.Lerp(yLow, yHigh, lerp);
    }

    /// <summary>
    /// Remaps the value <paramref name="value"/> from range X to range Y.
    /// </summary>
    public static Vector3 Remap(Vector3 xLow, Vector3 xHigh, Vector3 yLow, Vector3 yHigh, float value)
    {
        return new Vector3(
            Remap(xLow.x, xHigh.x, yLow.x, yHigh.x, value),
            Remap(xLow.y, xHigh.y, yLow.y, yHigh.y, value),
            Remap(xLow.z, xHigh.z, yLow.z, yHigh.z, value));
    }

    public static float CalculatePitchG(Transform transform, Vector3 velocity, float pitchRateDeg)
    {
        // Angular velocity is in radians per second.
        var pitchRate = pitchRateDeg * Mathf.Deg2Rad;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        // If there is no angular velocity in the pitch, then there's no force generated by a turn.
        // Return only the planet's gravity as it would be felt in the vertical.
        if (Mathf.Abs(pitchRate) < Mathf.Epsilon)
            return transform.up.y;

        // Local pitch velocity (X) is positive when pitching down.

        // Radius of turn = velocity / angular velocity
        float radius = localVelocity.z / pitchRate;

        // The radius of the turn will be negative when in a pitching down turn.

        // Force is mass * radius * angular velocity^2
        float verticalForce = (localVelocity.z * localVelocity.z) / radius;

        // Express in G
        float verticalG = -verticalForce / 9.8f;

        // Add the planet's gravity in. When the up is facing directly up, then the full
        // force of gravity will be felt in the vertical.
        verticalG += transform.up.y;

        return verticalG;
    }
}
