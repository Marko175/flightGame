using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camLook : MonoBehaviour
{
    public GameObject hud;
    public float sensitivityX = 30;
    public float sensitivityY = 30;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationX = 0F;
    float rotationY = 0F;
    Quaternion originalRotation;

    public float Response = 10;

    void Update()
    {
        RotateCamera();


    }
    
        void RotateCamera()
    {
        if (Input.GetKey(KeyCode.Mouse2))
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);

            


            Quaternion target = originalRotation * xQuaternion * yQuaternion;
            transform.localRotation = Damp(transform.localRotation, target, Response, Time.deltaTime);

            Debug.Log(target.eulerAngles.ToString());

            if (target.eulerAngles.y < 90 || target.eulerAngles.y > 270 || !(name.Equals("HudCam")))
                hud.SetActive(false);
            else
                hud.SetActive(true);


        }
        else if(!Input.GetKey(KeyCode.Mouse2))
        {
            rotationX = 0;
            rotationY = 0;

            if (name.Equals("HudCam"))
            {
                hud.SetActive(true);
            }

            Quaternion target = originalRotation;
            transform.localRotation = Damp(transform.localRotation, target, Response, Time.deltaTime);

        }


    }


        void Start()
    {
        originalRotation = transform.localRotation;
    }
    private Quaternion Damp(Quaternion a, Quaternion b, float lambda, float dt)
    {
        return Quaternion.Slerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }
}
