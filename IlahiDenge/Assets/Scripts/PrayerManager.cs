using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrayerManager : MonoBehaviour
{
    public GameObject prayerVisibilty;
    public GameObject startButton;
    public GameObject kuraklik;

    public Text prayText;
    public int randomNumber;
    public string[] sentences;
    int i = 0;
    public bool started = false;
    public bool choose;

    public float currentTime;
    public float startingTime;
    void Start()
    {
        sentences = new string[10];
        currentTime = startingTime;
        prayerVisibilty.SetActive(false);
    }

    void Update()
    {
        if (started)
        {
            currentTime -= 1 * Time.deltaTime;

            if (currentTime <= 0f)
                prayerVisibilty.SetActive(true);
        }
    }
    public void StartPray(Prayer prayer)
    {

        foreach(string sentence in prayer.sentences)
        {
            sentences[i] = sentence;
            i++;
        }
        started = true;
        DisplayNextPrayer();
        startButton.SetActive(false);
        kuraklik.SetActive(true);
    }

    public void DisplayNextPrayer()
    {
        randomNumber = Random.Range(0, 10);
        Debug.Log(randomNumber.ToString());

        if (randomNumber < 7)
            choose = true;
        else
            choose = false;

        Debug.Log(choose);
        string sentence = sentences[randomNumber];
        prayText.text = sentence;
        currentTime = 30f;
        prayerVisibilty.SetActive(false);
    }

    public void Accept()
    {
        FindObjectOfType<ProgressBar>().goodChoice(choose);
        DisplayNextPrayer();
        Debug.Log(choose);
    }

    public void Ignore()
    {
        FindObjectOfType<ProgressBar>().badChoice(choose);
        DisplayNextPrayer();
        Debug.Log(choose);
    }
}
