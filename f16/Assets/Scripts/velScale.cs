using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class velScale : MonoBehaviour
{

    public float height;
    public RectTransform rt;
    void Start()
    {

        rt = GetComponent<RectTransform>();
        height = 753f;
        Debug.Log(height);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sufa.Player == null)
            return;
        var player = sufa.Player;

        rt.localPosition = Vector3.down * (player.Speed)* height*1.94384f / 850f + Vector3.up * 372f;

    }
}
