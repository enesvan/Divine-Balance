using UnityEngine;

public class House : MonoBehaviour {

    public int capacity = 3;
    public int population = 0;

    private void Awake() {
        GetComponent<AudioSource>().volume = SoundManager.instance.sfxSound;
    }
}
