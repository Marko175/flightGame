using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCam : MonoBehaviour
{
    public Camera dc;
    public Vector3 StartPosition;
    public bool active;
    public static DeadCam dCam { get; private set; } = null;
    // Start is called before the first frame update
    void Start()
    {
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 1000)
            transform.position -= transform.forward * Time.deltaTime * 10f;
        transform.LookAt(GameObject.Find("BigExplosion").transform);
        transform.position += transform.right * Time.deltaTime * 5f;
        transform.position += Vector3.up * Time.deltaTime * 3f;

        dc.fieldOfView += 2 * Time.deltaTime;
    }
}
