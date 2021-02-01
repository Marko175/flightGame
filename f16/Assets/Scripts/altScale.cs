using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class altScale : MonoBehaviour
{
    public float height;
    public RectTransform rt;
    void Start()
    {

        rt = GetComponent<RectTransform>();
        height = 3011f;
       

    }

    // Update is called once per frame
    void Update()
    {
        if (sufa.Player == null)
            return;
        var player = sufa.Player;

        rt.localPosition = Vector3.down * (player.altitude) * height * 3.28084f / 32500f + Vector3.up * 1440f;

    }
}
