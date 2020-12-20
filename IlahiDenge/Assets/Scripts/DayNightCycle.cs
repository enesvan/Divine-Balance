using UnityEngine;
using Globals;

[ExecuteInEditMode]
public class DayNightCycle : MonoBehaviour {
    public static DayNightCycle instance;

    public AnimationCurve sunCurve;

    public float currentHour = 6;
    public float cycleSpeed = 0.1f;
    public bool isNight;

    public G.VoidDel Nightfall;
    public G.VoidDel Daylight;

    Light sun;
    bool isNightBefore;

    void Awake() {
        if (DayNightCycle.instance) {
            Destroy(this);
        }
        else {
            DayNightCycle.instance = this;
        }

        sun = GetComponent<Light>();
    }

    void Update () {
        //transform.Rotate(Vector3.right, cycleSpeed * Time.deltaTime);
        
        if (Application.isPlaying) {
            currentHour += cycleSpeed * Time.deltaTime;
            if (currentHour > 24) {
                currentHour = 0;
            }
        }
        

        isNight = currentHour < 6 || currentHour > 19;

        if (!isNightBefore && isNight) {
            Nightfall();
        }

        if (isNightBefore && !isNight) {
            Daylight();
        }

        float rotAmount = (-6 + currentHour) * 360 / 24;
        transform.rotation = Quaternion.Euler(rotAmount, 0, 0);

        if (isNight) {
            sun.intensity = 0;
        }
        else {
            sun.intensity = sunCurve.Evaluate(rotAmount / 180);
        }
        

        isNightBefore = isNight;
    }
}
