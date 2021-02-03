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

    [Header("PositiveSemiHorizon")]
    [SerializeField] private RectTransform five = null;
    [SerializeField] private RectTransform ten = null;
    [SerializeField] private RectTransform fifteen = null;
    [SerializeField] private RectTransform twenty = null;
    [SerializeField] private RectTransform twentyfive = null;
    [SerializeField] private RectTransform thirty = null;
    [SerializeField] private RectTransform thirtyfive = null;
    [SerializeField] private RectTransform fourty = null;
    [SerializeField] private RectTransform fourtyfive = null;
    [SerializeField] private RectTransform fifty = null;
    [SerializeField] private RectTransform fiftyfive = null;
    [SerializeField] private RectTransform sixty = null;
    [SerializeField] private RectTransform sixtyfive = null;
    [SerializeField] private RectTransform seventy = null;
    [SerializeField] private RectTransform seventyfive = null;
    [SerializeField] private RectTransform eighty = null;
    [SerializeField] private RectTransform eightyfive = null;
    [SerializeField] private RectTransform vertical = null;

    [Header("PositiveSemiHorizon_Over")]
    [SerializeField] private RectTransform sixty_over = null;
    [SerializeField] private RectTransform sixtyfive_over = null;
    [SerializeField] private RectTransform seventy_over = null;
    [SerializeField] private RectTransform seventyfive_over = null;
    [SerializeField] private RectTransform eighty_over = null;
    [SerializeField] private RectTransform eightyfive_over = null;



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


        GenerateHorizon(player);

        
        g.text = $"{player.g:0.0}G";


    }

    private void GenerateHorizon(sufa player)
    {
        Horizon.position = Camera.main.WorldToScreenPoint(player.transform.position + Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * new Vector3(player.transform.forward.x, 0, player.transform.forward.z)).normalized * scope);
        Horizon.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        float deg = 5;
        five.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0,0,0)*(new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized* Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        five.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = 10;
        ten.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        ten.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = 15;
        fifteen.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        fifteen.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = 20;
        twenty.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        twenty.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = 25;
        twentyfive.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        twentyfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = 30;
        thirty.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        thirty.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = 35;
        thirtyfive.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        thirtyfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = 40;
        fourty.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        fourty.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = 45;
        fourtyfive.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        fourtyfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = 50;
        fifty.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        fifty.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = 55;
        fiftyfive.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        fiftyfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = 60;
        sixty.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        sixty.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        sixty_over.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * -Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        sixty_over.rotation = Quaternion.Euler(0, 0, 180 - player.transform.rotation.eulerAngles.z);

        deg = 65;
        sixtyfive.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        sixtyfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        sixtyfive_over.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * -Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        sixtyfive_over.rotation = Quaternion.Euler(0, 0, 180 - player.transform.rotation.eulerAngles.z);

        deg = 70;
        seventy.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        seventy.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        seventy_over.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * -Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        seventy_over.rotation = Quaternion.Euler(0, 0, 180 - player.transform.rotation.eulerAngles.z);

        deg = 75;
        seventyfive.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        seventyfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        seventyfive_over.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * -Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        seventyfive_over.rotation = Quaternion.Euler(0, 0, 180 - player.transform.rotation.eulerAngles.z);

        deg = 80;
        eighty.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        eighty.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        eighty_over.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * -Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        eighty_over.rotation = Quaternion.Euler(0, 0, 180 - player.transform.rotation.eulerAngles.z);

        deg = 85;
        eightyfive.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        eightyfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        eightyfive_over.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * -Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        eightyfive_over.rotation = Quaternion.Euler(0, 0, 180-player.transform.rotation.eulerAngles.z);

        deg = 90;
        vertical.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        vertical.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        if (player.transform.forward.y < 0)
        {
            vertical.position = thirty.position;
            sixty.position = thirty.position;
            sixty_over.position = thirty.position;
            sixtyfive.position = thirty.position;
            sixtyfive_over.position = thirty.position;
            seventy.position = thirty.position;
            seventy_over.position = thirty.position;
            seventyfive.position = thirty.position;
            seventyfive_over.position = thirty.position;
            eighty.position = thirty.position;
            eighty_over.position = thirty.position;
            eightyfive.position = thirty.position;
            eightyfive_over.position = thirty.position;
        }


    }
}
