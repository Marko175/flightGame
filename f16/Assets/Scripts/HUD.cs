using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections.Generic;

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
    public float scope = 8f;
    sufa player;


    [SerializeField] private Text velocity;
    [SerializeField] private Text g;
    [SerializeField] private Text angle;
    [SerializeField] private Text altitude;
    [SerializeField] private Text rAlt;
    [SerializeField] private LineRenderer lineR;
    [SerializeField] private LineRenderer lineTLL;

    [Header("Flight Elements")]
    [SerializeField] private RectTransform FPM = null;
    [SerializeField] private RectTransform Horizon = null;
    [SerializeField] private RectTransform Piper = null;
    private List<Vector3> bulletPositions;
    [SerializeField] private GameObject Horizons;
    [SerializeField] private GameObject negHorizons;
    [SerializeField] private GameObject TLL;
    [SerializeField] private Quaternion TLLRotation;
    [SerializeField] private RectTransform TDB;
    [SerializeField] private Text FCRDistance;
    [SerializeField] private Text FCRClosing;

    [Header("Ticks")]
    [SerializeField] private RectTransform Tick1;
    [SerializeField] private RectTransform Tick2;
    [SerializeField] private RectTransform Tick3;
    [SerializeField] private RectTransform Tick4;


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

    [Header("NegSemiHorizon")]
    [SerializeField] private RectTransform nvertical = null;
    [SerializeField] private RectTransform nfive = null;
    [SerializeField] private RectTransform nten = null;
    [SerializeField] private RectTransform nfifteen = null;
    [SerializeField] private RectTransform ntwenty = null;
    [SerializeField] private RectTransform ntwentyfive = null;
    [SerializeField] private RectTransform nthirty = null;
    [SerializeField] private RectTransform nthirtyfive = null;
    [SerializeField] private RectTransform nfourty = null;
    [SerializeField] private RectTransform nfourtyfive = null;
    [SerializeField] private RectTransform nfifty = null;
    [SerializeField] private RectTransform nfiftyfive = null;
    [SerializeField] private RectTransform nsixty = null;
    [SerializeField] private RectTransform nsixtyfive = null;
    [SerializeField] private RectTransform nseventy = null;
    [SerializeField] private RectTransform nseventyfive = null;
    [SerializeField] private RectTransform neighty = null;
    [SerializeField] private RectTransform neightyfive = null;


    [Header("NegSemiHorizon_Over")]
    [SerializeField] private RectTransform nsixty_over = null;
    [SerializeField] private RectTransform nsixtyfive_over = null;
    [SerializeField] private RectTransform nseventy_over = null;
    [SerializeField] private RectTransform nseventyfive_over = null;
    [SerializeField] private RectTransform neighty_over = null;
    [SerializeField] private RectTransform neightyfive_over = null;



    // Start is called before the first frame update
    void Start()
    {
        player = sufa.Player;
        bulletPositions = new List<Vector3>();
        bulletPositions.Add(Vector3.zero);
        bulletPositions.Add(Vector3.zero);
        bulletPositions.Add(Vector3.zero);
        TLLRotation = TLL.transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player == null)
            return;
        

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
        FPM.gameObject.SetActive(player.mode == 1);

        if (player.target == null)
        {
            TDB.gameObject.SetActive(false);
            FCRDistance.gameObject.SetActive(false);
        }
        else {
            TDB.gameObject.SetActive(true);
            FCRDistance.gameObject.SetActive(true);
            FCRDistance.text = (player.targetDistance/30).ToString("n0");
            FCRClosing.text = (Mathf.Round(Units.toKnots((player.closeVelocity))/10) * 10).ToString("0");
            TDB.position = Camera.main.WorldToScreenPoint(player.transform.position + ((player.target.transform.position-player.transform.position).normalized *30f));
        }


        GenerateTLL();
        GenerateHorizon(player);
        GenerateNegHorizon(player);
        CalculatePiper();

        Horizons.gameObject.SetActive(player.mode == 1);
        Horizon.gameObject.SetActive(player.mode == 1);
        negHorizons.gameObject.SetActive(player.mode == 1);

        
        g.text = $"{player.g:0.0}G";

        this.transform.position = Camera.main.WorldToScreenPoint(player.transform.position +player.transform.forward * scope *120);


    }

    private void GenerateTLL()
    {
        if (player.target == null)
            return;
        TLL.GetComponent<LineRenderer>().enabled = (player.target != null);
        lineTLL.startWidth = 0.0015f;
        lineTLL.endWidth = 0.0015f;
        lineTLL.positionCount = 2;
        lineTLL.SetPosition(0, new Vector3(0,0, 0));
        Vector3 tll = Vector3.ProjectOnPlane((player.target.transform.position - TLL.transform.position), player.transform.forward).normalized * 0.05f ;
        TLL.transform.rotation = TLLRotation;

        lineTLL.enabled = !RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), TDB.position);
        float a = Mathf.Round(Vector3.Angle(player.transform.position + player.transform.forward, player.transform.position + (player.target.transform.position - player.transform.position))/10) *10;
        TDB.gameObject.SetActive(RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), TDB.position));
        angle.gameObject.SetActive(!RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), TDB.position));
        if (a > 90)
        {
            TDB.gameObject.SetActive(false);
            angle.gameObject.SetActive(true);
        }

        angle.text = a.ToString("n0");


        lineTLL.SetPosition(1, tll);
    }

    private void CalculatePiper()
    {
        Piper.gameObject.SetActive(player.mode ==2);
        Tick1.gameObject.SetActive(player.mode ==2);
        Tick2.gameObject.SetActive(player.mode ==2);
        Tick3.gameObject.SetActive(player.mode ==2);
        Tick4.gameObject.SetActive(player.mode ==2);

        GetComponent<LineRenderer>().enabled = (player.mode == 2);
        
        Vector3 bulletPosition = player.gun.transform.position;
        Vector3 vel = player.gun.transform.forward * player.bulletSpeed;
        float time = Time.fixedDeltaTime;
        float timeCount =0;
        float targetTime = 0;
        float dis = 300;

        if (player.target != null)
            dis = player.targetDistance;


        while ((bulletPosition - player.transform.position).magnitude < 500)
        {
            vel += Physics.gravity * time;
            bulletPosition += vel * time;
            timeCount += time;

            if(player.targetDistance< (bulletPosition - player.transform.position).magnitude)
                    targetTime += time;
        }

        float piperFrame = dis / 500;
        if (piperFrame > 1)
            piperFrame = 1;




        bulletPosition = bulletPosition.normalized * 500f;
        bulletPosition = Vector3.Slerp(bulletPosition, bulletPositions[bulletPositions.Count-1], Time.deltaTime *20f);
        bulletPositions.Add(bulletPosition);

        if (bulletPositions.Count > 40)
            bulletPositions.RemoveAt(0);

        if (bulletPositions.Count > 33)
        {
            if(player.target == null)
                Piper.position = Camera.main.WorldToScreenPoint(bulletPositions[19]);
            else
                Piper.position = Camera.main.WorldToScreenPoint(bulletPositions[39-Mathf.RoundToInt(piperFrame * 39)]);

            Tick1.position = Camera.main.WorldToScreenPoint(bulletPositions[33]);
            Tick2.position = Camera.main.WorldToScreenPoint(bulletPositions[26]);
            Tick3.position = Camera.main.WorldToScreenPoint(bulletPositions[19]);
            Tick4.position = Camera.main.WorldToScreenPoint(bulletPositions[12]);

        }
        
        lineR.startWidth = 1.4f;
        lineR.endWidth = 1.4f;
        lineR.positionCount = bulletPositions.Count;
        lineR.SetPositions(bulletPositions.ToArray());
    }

   

    private void GenerateNegHorizon(sufa player)
    {


        float deg = -5;
        nfive.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = -10;
        nten.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nten.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = -15;
        nfifteen.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nfifteen.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = -20;
        ntwenty.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        ntwenty.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = -25;
        ntwentyfive.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        ntwentyfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = -30;
        nthirty.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nthirty.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = -35;
        nthirtyfive.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nthirtyfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = -40;
        nfourty.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nfourty.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = -45;
        nfourtyfive.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nfourtyfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = -50;
        nfifty.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nfifty.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = -55;
        nfiftyfive.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nfiftyfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        deg = -60;
        nsixty.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nsixty.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        nsixty_over.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * -Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nsixty_over.rotation = Quaternion.Euler(0, 0, 180 - player.transform.rotation.eulerAngles.z);

        deg = -65;
        nsixtyfive.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nsixtyfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        nsixtyfive_over.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * -Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nsixtyfive_over.rotation = Quaternion.Euler(0, 0, 180 - player.transform.rotation.eulerAngles.z);

        deg = -70;
        nseventy.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nseventy.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        nseventy_over.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * -Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nseventy_over.rotation = Quaternion.Euler(0, 0, 180 - player.transform.rotation.eulerAngles.z);

        deg = -75;
        nseventyfive.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nseventyfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        nseventyfive_over.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * -Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nseventyfive_over.rotation = Quaternion.Euler(0, 0, 180 - player.transform.rotation.eulerAngles.z);

        deg = -80;
        neighty.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        neighty.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        neighty_over.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * -Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        neighty_over.rotation = Quaternion.Euler(0, 0, 180 - player.transform.rotation.eulerAngles.z);

        deg = -85;
        neightyfive.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        neightyfive.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        neightyfive_over.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * -Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        neightyfive_over.rotation = Quaternion.Euler(0, 0, 180 - player.transform.rotation.eulerAngles.z);

        deg = -90;
        nvertical.position = Camera.main.WorldToScreenPoint(player.transform.position + (Vector3.Project(player.transform.forward, Quaternion.Euler(0, 0, 0) * (new Vector3(player.transform.forward.x, 0, player.transform.forward.z))).normalized * Mathf.Cos(Mathf.Deg2Rad * deg) + new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * deg), 0)) * scope);
        nvertical.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);

        if (player.transform.forward.y > 0)
        {
            nsixty.position = nthirty.position;
            nsixty_over.position = nthirty.position;
            nsixtyfive.position = nthirty.position;
            nsixtyfive_over.position = nthirty.position;
            nseventy.position = nthirty.position;
            nseventy_over.position = nthirty.position;
            nseventyfive.position = nthirty.position;
            nseventyfive_over.position = nthirty.position;
            neighty.position = nthirty.position;
            neighty_over.position = nthirty.position;
            neightyfive.position = nthirty.position;
            neightyfive_over.position = nthirty.position;
            nvertical.position = nthirty.position;
        }
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
