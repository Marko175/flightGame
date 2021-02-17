using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeTarget : MonoBehaviour
{
    public float yaw;
    public float vel;
    public ParticleSystem explode;
    // Start is called before the first frame update
    void Start()
    {
        vel = 120;
        yaw = -10;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += vel * Time.deltaTime * transform.forward;


        var yawRotation = Quaternion.AngleAxis(yaw * Time.deltaTime, Vector3.right);
        
        transform.localRotation *= yawRotation;


    }

    public void Die()
    {
        ParticleSystem e = Instantiate(explode);
        e.transform.position = transform.position;
        e.Play();

//        Destroy(gameObject);
    }
}
