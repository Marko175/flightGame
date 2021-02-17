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
        Debug.Log((other.gameObject.transform.position - sufa.Player.transform.position).magnitude);
        if (other.gameObject.tag == "enemy")
            other.gameObject.GetComponentInParent<PracticeTarget>().Die();
    }
}
