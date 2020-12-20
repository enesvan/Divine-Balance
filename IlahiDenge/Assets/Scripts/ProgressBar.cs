using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;
    public float decreaseRate;
    private float targetProgress = 0;

    public Text faithAmountText;

    public float goodProgress;
    public float badProgress;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }
    void Start()
    {
        IncreaseProgress(1f);
    }

    
    void Update()
    {
        if (slider.value < targetProgress && FindObjectOfType<PrayerManager>().started)
            slider.value -= decreaseRate * Time.deltaTime;
        /*if (slider.value < 0.2f || slider.value > 0.8f)
            Debug.Log("GAME OVER");*/
        faithAmountText.text = "%" + Math.Round(slider.value * 100, 0);
        if ((float)GameManager.instance.foodCount < (float)GameManager.instance.population / 2)
            slider.value -= decreaseRate * Time.deltaTime;

    }

    public void IncreaseProgress(float incProgress)
    {
        targetProgress = slider.value + incProgress;
    }

    public void DecreaseProgress(float decProgress)
    {
        targetProgress = slider.value - decProgress;
    }

    public void goodChoice(bool choose)
    {
        if (choose)
            slider.value = slider.value + goodProgress + 0.01f;
        else
            slider.value = slider.value - badProgress - 0.01f;
    }

    public void badChoice(bool choose)
    {
        if (choose)
            slider.value = slider.value - badProgress - 0.01f;
        else
            slider.value = slider.value + goodProgress + 0.01f;
    }

    public void FaithUpgrade(int variant)
    {
        switch (variant)
        {
            case 1:
                {
                    slider.value += 0.01f;
                    break;
                }
            case 2:
                {
                    slider.value += 0.04f;
                    break;
                }
            case 3:
                {
                    slider.value += 0.01f;
                    break;
                }
            default:
                break;
        }
    }
}
