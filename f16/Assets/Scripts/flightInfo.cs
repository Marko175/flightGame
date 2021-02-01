using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class flightInfo : MonoBehaviour
{
    public Text Throttle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sufa.Player == null)
            return;
        var player = sufa.Player;

        Throttle.text = "Throttle: " + Mathf.RoundToInt(player.engThrottle * 100) + "%";

        if (player.ab)
        {
            Throttle.text = "Pilpell";
        }
    }
}
