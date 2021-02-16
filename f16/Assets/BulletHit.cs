using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var r = other.gameObject.GetComponent<Renderer>();
        r.material.SetColor("rand",Color.red);

        other.gameObject.transform.localScale *= 0.5f;
    }
}
