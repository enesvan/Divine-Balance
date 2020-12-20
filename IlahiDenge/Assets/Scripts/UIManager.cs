using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    public Text kuraklikText;

    public GameObject optionsMenu;
    public GameObject mainMenu;

    public Slider musicSlider;
    public Slider sfxSlider;


    private void Awake() {
        if (UIManager.instance) {
            Destroy(this);
        }
        else {
            UIManager.instance = this;
        }


        Time.timeScale = 0;

        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void StartGame () {
        Time.timeScale = 1;
        FindObjectOfType<PrayerTrigger>().TriggerPrayer();
        mainMenu.SetActive(false);
    }

    public void OpenOptions () {
        optionsMenu.SetActive(true);
        if (!mainMenu.activeSelf)
            Time.timeScale = 0;
    }

    public void CloseOptions() {
        optionsMenu.SetActive(false);
        if (!mainMenu.activeSelf)
            Time.timeScale = 1;
    }

    public void QuitGame () {
        Application.Quit();
    }



    public void RainButton () {
        GameManager.instance.StartCoroutine("Rain");
    }

    private void Update()
    {
        kuraklikText.text = "Kuraklık: %" + Math.Round((5f - GameManager.instance.waterLevel)*4,0);

        SoundManager.instance.musicSound = musicSlider.value;
        SoundManager.instance.sfxSound = sfxSlider.value;
    }
}
