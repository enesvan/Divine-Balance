using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager instance;

    [Range(0,1)]
    public float ambientSound = 0.8f;
    [Range(0, 1)]
    public float musicSound = 0.8f;
    [Range(0, 1)]
    public float sfxSound = 0.5f;
    [Range(0, 1)]
    public float rainSound = 0.5f;

    public AudioSource music, day, night, rain, spawn;
    public AudioSource click, pause;

    void Awake() {
        if (SoundManager.instance) {
            Destroy(this);
        }
        else {
            SoundManager.instance = this;
        }
    }

    private void Start () {
        DayNightCycle dnm = DayNightCycle.instance;

        dnm.Nightfall += () => {
            day.volume = 0;
            night.volume = ambientSound;
        };
        dnm.Daylight += () => {
            day.volume = ambientSound;
            night.volume = 0;
        };

        if (dnm.isNight) {
            day.volume = 0;
            night.volume = ambientSound;
        }
        else {
            day.volume = ambientSound;
            night.volume = 0;
        }
    }

    private void Update() {
        music.volume = musicSound;

        spawn.volume = sfxSound;
        click.volume = sfxSound;
        pause.volume = sfxSound;
    }

    public void RainSound (bool val) {
        rain.volume = val ? rainSound : 0;
    }

    
}
