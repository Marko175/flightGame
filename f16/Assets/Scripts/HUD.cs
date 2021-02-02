using UnityEngine.UI;
using UnityEngine;
using System;

public static class Units
{
    public static float toKnots(float mps)
    {
        return mps * 1.94384f;
    }

    public static float toFeet(float meters)
    {
        return meters * 3.28084f;
    }
}

public class HUD : MonoBehaviour
{
    private float nextActionTime = 0.0f;
    public float period = 0.2f;
    public float scope = 5f;

    [SerializeField] private Text velocity;
    [SerializeField] private Text g;
    [SerializeField] private Text altitude;
    [SerializeField] private Text rAlt;

    [Header("Flight Elements")]
    [SerializeField] private RectTransform FPM = null;
    [SerializeField] private RectTransform Horizon = null;

    [Header("SemiHorizon")]
    [SerializeField] private RectTransform five = null;
    [SerializeField] private RectTransform ten = null;
    [SerializeField] private RectTransform fifteen = null;
    [SerializeField] private RectTransform twenty = null;
    [SerializeField] private RectTransform twentyfive = null;
    [SerializeField] private RectTransform thirty = null;
    [SerializeField] private RectTransform thirtyfive = null;
    [SerializeField] private RectTransform fourty = null;
    [SerializeField] private RectTransform fourtyfive = null;



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

        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            velocity.text = Units.toKnots(player.Speed).ToString("0");
            
            altitude.text = (Mathf.Round(Units.toFeet(player.altitude) / 10) * 10).ToString("n0");

            if (player.rAltitude == -999999f)
                rAlt.text = "";
            else
                rAlt.text = (Mathf.Round(Units.toFeet(player.rAltitude) / 10) * 10).ToString("n0");
        }

        var velocityPos = player.transform.position + player.VelocityDirection * scope;
        FPM.position = Camera.main.WorldToScreenPoint(velocityPos);


        Horizon.position = Camera.main.WorldToScreenPoint(player.transform.position + Vector3.Project(player.transform.forward, Quaternion.Euler(0,0,0)*new Vector3(player.transform.forward.x,0,player.transform.forward.z)).normalized * scope);
        Horizon.rotation =  Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        GenerateSemiHorizon(player);

        
        g.text = $"{player.g:0.0}G";
        Debug.Log(player.transform.rotation.x);

    }

    private void GenerateSemiHorizon(sufa player)
    {
        float deg = -5;
        five.position = player.transform.position + Vector3.Project(player.transform.forward, (new Vector3(player.transform.forward.x, 0, player.transform.forward.z)).normalized*Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope;
        five.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);
        five.position = Camera.main.WorldToScreenPoint(five.position);

        deg = 10;
        ten.position = player.transform.position + Vector3.Project(player.transform.forward, (new Vector3(player.transform.forward.x, 0, player.transform.forward.z)).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope;
        ten.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);
        ten.position = Camera.main.WorldToScreenPoint(ten.position);

        deg = 15;
        fifteen.position = player.transform.position + Vector3.Project(player.transform.forward, (new Vector3(player.transform.forward.x, 0, player.transform.forward.z)).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg) ,0))* scope;
        fifteen.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);
        fifteen.position = Camera.main.WorldToScreenPoint(fifteen.position);

        deg = 20;
        twenty.position = player.transform.position + Vector3.Project(player.transform.forward, (new Vector3(player.transform.forward.x, 0, player.transform.forward.z)).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope;
        twenty.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);
        twenty.position = Camera.main.WorldToScreenPoint(twenty.position);

        deg = 25;
        twentyfive.position = player.transform.position + Vector3.Project(player.transform.forward, (new Vector3(player.transform.forward.x, 0, player.transform.forward.z)).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope;
        twentyfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);
        twentyfive.position = Camera.main.WorldToScreenPoint(twentyfive.position);

        deg = 30;
        thirty.position = player.transform.position + Vector3.Project(player.transform.forward, (new Vector3(player.transform.forward.x, 0, player.transform.forward.z)).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope;
        thirty.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);
        thirty.position = Camera.main.WorldToScreenPoint(thirty.position);

        deg = 35;
        thirtyfive.position = player.transform.position + Vector3.Project(player.transform.forward, (new Vector3(player.transform.forward.x, 0, player.transform.forward.z)).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope;
        thirtyfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);
        thirtyfive.position = Camera.main.WorldToScreenPoint(thirtyfive.position);

        deg = 40;
        fourty.position = player.transform.position + Vector3.Project(player.transform.forward, (new Vector3(player.transform.forward.x, 0, player.transform.forward.z)).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope;
        fourty.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);
        fourty.position = Camera.main.WorldToScreenPoint(fourty.position);

        deg = 45;
        fourtyfive.position = player.transform.position + Vector3.Project(player.transform.forward, (new Vector3(player.transform.forward.x, 0, player.transform.forward.z)).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope;
        fourtyfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);
        fourtyfive.position = Camera.main.WorldToScreenPoint(fourtyfive.position);

    }
}
