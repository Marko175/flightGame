using UnityEngine.UI;
using UnityEngine;

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

    [SerializeField] private Text velocity;
    [SerializeField] private Text g;
    [SerializeField] private Text altitude;
    [SerializeField] private Text rAlt;

    [Header("Flight Elements")]
    [SerializeField] private RectTransform FPM = null;
    [SerializeField] private RectTransform Horizon = null;



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

        var velocityPos = player.transform.position + player.VelocityDirection * 500f;
        FPM.position = Camera.main.WorldToScreenPoint(velocityPos);


        Horizon.position = Camera.main.WorldToScreenPoint(player.transform.position + Vector3.Project(player.transform.forward, Quaternion.Euler(0,0,0)*new Vector3(player.transform.forward.x,0,player.transform.forward.z)).normalized * 50f);
        Horizon.rotation =  Quaternion.Euler(0, 0, -player.transform.rotation.eulerAngles.z);
        


        g.text = $"{player.g:0.0}G";

    }
}
