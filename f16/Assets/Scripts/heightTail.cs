using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heightTail : MonoBehaviour
{
    public float maxTail = 15f;
    public float tailAcc = 3f;
    private float tail = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float targetPitch = 0;
        if (Input.GetAxis("Vertical") > 0)
            targetPitch = maxTail/4;
        else if (Input.GetAxis("Vertical") < 0)
            targetPitch = -maxTail;
        tail = SmoothDamp.Move(tail, targetPitch, tailAcc, Time.deltaTime);
        var pitchRotation = Quaternion.AngleAxis(tail , Vector3.right);
        transform.localRotation = pitchRotation;
    }
}
