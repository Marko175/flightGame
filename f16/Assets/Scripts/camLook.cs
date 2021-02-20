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
    float originalZoom;
    Vector3 originalScale;
    Vector3 updatedScale;
    float vel1;
    public float hudZoom;
    public float updatedZoom;
    public Camera cam;
    bool zoomed;


    public float Response = 3;
    void Start()
    {
        originalRotation = transform.localRotation;
        hudZoom = 1.43f;
        originalZoom = cam.fieldOfView;
        originalScale = hud.transform.localScale;
        updatedScale = hud.transform.localScale;
        updatedZoom = cam.fieldOfView;
        zoomed = false;

    }
    void FixedUpdate()
    {
        
        RotateCamera();
        if (Input.GetKey(KeyCode.Mouse1))
        {
            zoomed = true;
            float targetZoom = originalZoom - 15f;
            cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, targetZoom, ref vel1, Time.deltaTime, 100);
            float ratio1 = (cam.fieldOfView - originalZoom) / (targetZoom - originalZoom);


            Vector3 targetScale = originalScale * hudZoom;
            hud.transform.localScale = originalScale + (targetScale-originalScale) * ratio1;
            updatedScale = hud.transform.localScale;
            updatedZoom = cam.fieldOfView;


        }
        else if (zoomed)
        {
            float targetZoom = originalZoom;
            cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, targetZoom, ref vel1, Time.deltaTime, 100);
            float ratio2 = (cam.fieldOfView - targetZoom) / (updatedZoom - targetZoom);

            hud.transform.localScale = originalScale + (updatedScale - originalScale) * ratio2;
            vel1 = 0;

        }


    }
    
        void RotateCamera()
    {
        if (Input.GetKey(KeyCode.Mouse2))
        {

            Quaternion target = transform.rotation;
            if (sufa.Player.target != null)
            {
                Vector3 relativePos = sufa.Player.target.transform.position - transform.position;
                if(Vector3.Angle(transform.forward, sufa.Player.target.transform.position)<90)
                    target = Quaternion.LookRotation(relativePos, transform.up);
                else
                    target = Quaternion.LookRotation(relativePos, -transform.up);
                transform.rotation = Damp(transform.rotation, target, Response, Time.deltaTime * 0.4f);
            }
            else
            {
                rotationX += Input.GetAxis("Mouse X") * sensitivityX;
                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
                target = originalRotation * xQuaternion * yQuaternion;
                transform.localRotation = Damp(transform.localRotation, target, Response, Time.deltaTime * 0.7f);
            }


            if (Vector3.Angle(transform.forward, sufa.Player.transform.forward)>80 || !(name.Equals("HudCam")))
            {
                hud.SetActive(false);
            }
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
            transform.localRotation = Damp(transform.localRotation, target, Response, Time.deltaTime*0.5f);

        }


    }


    
    private Quaternion Damp(Quaternion a, Quaternion b, float lambda, float dt)
    {
        return Quaternion.Slerp(a, b, 1 - Mathf.Exp(-lambda * dt));
    }

}
