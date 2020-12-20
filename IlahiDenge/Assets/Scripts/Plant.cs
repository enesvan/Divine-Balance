using UnityEngine;
using System.Collections;
using Globals;

public class Plant : MonoBehaviour {
    
    public AudioClip deathClip;
    [Range(0,1)]
    public float vol = 0;

    public float health = 10;
    public float drainSpeed = 0.1f;
    public float damageOnDrought = 0.01f;

    

    public G.VoidDel OnDeath;

    AudioSource audioSrc;
    GameManager gm;

    private void Awake() {
        audioSrc = GetComponent<AudioSource>();
        

        OnDeath += () => {
            var audioSource = new GameObject("bush_dead").AddComponent<AudioSource>();
            audioSource.clip = deathClip;
            audioSource.volume = SoundManager.instance.sfxSound;
            audioSource.Play();
        };
    }

    private void Start() {
        gm = GameManager.instance;
    }

    private void Update() {
        audioSrc.volume = vol * SoundManager.instance.sfxSound;

        if (health <= 0) {
            OnDeath();
            Destroy(this.gameObject);
        }

        gm.waterLevel -= drainSpeed * Time.deltaTime;
        if (gm.waterLevel < gm.droughtLevel) {
            health -= damageOnDrought * Time.deltaTime * -gm.waterLevel;
        }
    }
}
