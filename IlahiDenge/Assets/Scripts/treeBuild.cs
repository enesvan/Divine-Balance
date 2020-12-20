using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class treeBuild : MonoBehaviour
{
    public Vector3 place;
    public GameObject sapling;
    public GameObject bush;
    public GameObject dodo;

    public Text countSapling;
    public Text countBush;
    public Text countDodo;

    public int saplingCount;
    public int bushCount;
    public int dodoCount;

    private RaycastHit hit;

    public int variant;
    public float timer;

    public bool placeControl;
    public bool timerControl=true;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && placeControl == true)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform.tag == "terrain")
                {
                    place = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                    switch (variant)
                    {
                        case 1:
                            {
                                if (saplingCount > 0)
                                {
                                    SoundManager.instance.spawn.Play();
                                    Instantiate(sapling, place, Quaternion.Euler(-90, 0, 0));
                                    FindObjectOfType<ProgressBar>().FaithUpgrade(variant);
                                    saplingCount--;
                                }
                                break;
                            }
                        case 2:
                            {
                                if (dodoCount > 0)
                                {
                                    Instantiate(dodo, place, Quaternion.Euler(-90, 0, 0));
                                    FindObjectOfType<ProgressBar>().FaithUpgrade(variant);
                                    dodoCount--;
                                }
                                break;
                            }
                        case 3:
                            {
                                if (bushCount > 0)
                                {
                                    SoundManager.instance.spawn.Play();
                                    Instantiate(bush, place, Quaternion.Euler(-90, 0, 0));
                                    FindObjectOfType<ProgressBar>().FaithUpgrade(variant);
                                    bushCount--;
                                }
                                break;
                            }
                        default:
                            break;
                    }
                    placeControl = false;
                }
            }
        }
        timer -= Time.deltaTime;
        if (FindObjectOfType<ProgressBar>().slider.value > 0.5f && timerControl && timer <= 0f)
        {
            saplingCount += 2;
            bushCount += 3;
            dodoCount += 1;
            timerControl = false;
            timer = 35f;
        }
        else
            timerControl = true;

        countSapling.text = saplingCount.ToString();
        countBush.text = bushCount.ToString();
        countDodo.text = dodoCount.ToString();

    }

    public void Place()
    {
        variant = 1;
        placeControl = true;
    }

    public void PlaceDodo()
    {
        variant = 2;
        placeControl = true;
    }

    public void PlaceBush()
    {
        variant = 3;
        placeControl = true;
    }

}
