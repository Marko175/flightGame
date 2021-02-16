using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatingPointError : MonoBehaviour
{
    sufa player;
    public float threshhold;
    public bool moveing;
    // Start is called before the first frame update
    void Start()
    {
        player = sufa.Player;
        threshhold = 1f;
        moveing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;

        if (player.transform.position.magnitude > threshhold)
            ShiftWorld();
        else
            moveing = false;

    }

    private void ShiftWorld()
    {
        moveing = true;
        transform.position -= player.transform.position;
        UnityEngine.Object[] objects = FindObjectsOfType(typeof(Transform));
            foreach(UnityEngine.Object o in objects)
            {
                Transform t = (Transform)o;
                if (t.parent == null)
                {
                    t.position -= player.transform.position;
                }
            }
    }
}
