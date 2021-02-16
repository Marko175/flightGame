using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeTarget : MonoBehaviour
{
    public float yaw;
    public float vel;
    // Start is called before the first frame update
    void Start()
    {
        vel = Random.Range(100,200);
        yaw = Random.Range(-10, 10);
        transform.localScale *= Random.Range(1, 2);
        transform.position = new Vector3(Random.Range(-500,500), Random.Range(100,1000), Random.Range(-500,500));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += vel * Time.deltaTime * transform.forward;


        var yawRotation = Quaternion.AngleAxis(yaw * Time.deltaTime, Vector3.up);


        transform.localRotation *= yawRotation;

    }
}
