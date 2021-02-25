using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDelay : MonoBehaviour
{
    // Start is called before the first frame update
    public float delayTime;
    public float killTime;
    float time;
    void Start()
    {
        delayTime = 0.1f;
        killTime = 2.5f;
        
        if(GetComponent<BoxCollider>()!= null)
            Invoke("EnableBoxCollider", delayTime);
        Invoke("Kill", killTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void EnableBoxCollider()
    {
        GetComponent<BoxCollider>().enabled = true;
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
