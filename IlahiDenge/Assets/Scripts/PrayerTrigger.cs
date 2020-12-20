using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrayerTrigger : MonoBehaviour
{
    public Prayer prayer;

    public void TriggerPrayer()
    {
        FindObjectOfType<PrayerManager>().StartPray(prayer);
    }
}
