using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public ParticleSystem groundHit;
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
        if (other.gameObject.tag == "enemy")
            other.gameObject.GetComponentInParent<sufa>().Die();
        if (other.gameObject.name == "Terrain")
        {
            ParticleSystem e = Instantiate(groundHit);
            e.transform.position = transform.position;
            e.Play();
        }
        Destroy(gameObject);

    }
}
