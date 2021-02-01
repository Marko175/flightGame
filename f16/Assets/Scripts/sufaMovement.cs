using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sufaMovement : MonoBehaviour
{
    public float throttle;
    public float rollSpeed;
    public float pitchSpeed;
    public float gravity;
    public float cFactor;
    public float liftAcc;
    public float dragAcc;
    public float cd;
    public float cl;
    public float aoa;
    public float aos;
    public float vel;
    public float throttleSens;
    public float dragNum;
    public float liftNum;
    public Vector3 speed;
    public Vector3 acceleration;


    // Start is called before the first frame update
    void Start()
    {
        speed = -transform.forward * 300f;
        throttle = 100f;
        rollSpeed = 2f;
        pitchSpeed = 0.6f;
        throttleSens = 5f;
        gravity = -120f;
        dragNum = 0.052f;
        liftNum = 0.005f;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move();
        rotate();
    }


    public void move()
    {
        speed = calcVelocity();
        transform.position += speed * Time.deltaTime;
        vel = Vector3.Project(speed, transform.forward).magnitude;
    }

    private Vector3 calcVelocity()
    {
        Vector3 thrust = -transform.forward * calcThrust();
        Vector3 drag = -speed.normalized * calcDrag();
        dragAcc = drag.magnitude;
        Vector3 lift = transform.up * calcLift();
        liftAcc = lift.magnitude;
        Vector3 g = new Vector3(0, gravity, 0);

        acceleration = thrust + drag + lift + g;
        return acceleration * Time.deltaTime + speed;
    }

    private float calcLift()
    {
        aos = 90f-Vector3.SignedAngle(speed, -transform.right, transform.up);
        aoa = 90f-Vector3.SignedAngle(speed, -transform.up, -transform.right);
        cl = calcCL();
        return cl * vel * vel * liftNum;
    }

    private float calcCL()
    {
        if (aoa < -2)
            return 0.02f;
        else if (aoa < 1)
            return 0.08f;
        else if (aoa < 5)
            return 0.2f;
        else if (aoa < 10)
            return 0.6f;
        else if (aoa < 20)
            return 1.3f;
        else if (aoa < 30)
            return 1.6f;
        else if (aoa < 45)
            return 2f;
        else
            return 0;

    }

    private float calcDrag()
    {
        if (aoa > 2)
            cd = cl / 20;
        else
            cd = 0.08f / 20f;
        return cd * vel * vel*dragNum;
    }

    

    private float calcThrust()
    {
        float a = Input.GetAxis("Fire3");
        if(a<0 && throttle>0)
            throttle += a * throttleSens;
        if (a > 0 && throttle < 2000)
            throttle += a * throttleSens;
        return throttle;
    }

    public void rotate()
    {
    
        transform.Rotate(calcPitchSpeed() * -Input.GetAxis("Vertical"), Vector3.Project(transform.right, Vector3.down).y * 0.2f, calcRotSpeed() * Input.GetAxis("Horizontal")); ;
        
    }

    private float calcPitchSpeed()
    {
        if (aoa < 2)
        {
            if (Input.GetAxis("Vertical") > 0)
                if (aoa < -2)
                    return 0.02f;
                else
                    return 0.6f;
            else
                return 0.6f;
        }
        else if (aoa < 6)
            return 0.5f;
        else if (aoa < 12)
            return 0.4f;
        else if (aoa < 18)
            return 0.35f;
        else if (aoa < 22)
            return 0.3f;
        else if (aoa < 30)
            return 0.2f;
        else if (aoa < 40)
            return 0.1f;
        else return 0.02f;
        //return pitchSpeed;
    }

    private float calcRotSpeed() //to be calculated
    {
        return rollSpeed; ;
    }
}
