using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    float maxWater = 5;
    float minWater = -20;
    [Range(-20, 5)]
    public float waterLevel = -3;
    public float droughtLevel = -6;

    public float rainIncrease = 0.3f;
    public Transform waterPlane;
    public GameObject rain;

    public GameObject male;
    public GameObject female;

    public GameObject house;
    public GameObject housePlaceholder;

    public GameObject[] trees;

    public int population;
    public int dodoPopulation;
    public int foodCount = 4;
    public int foodCountForBreed = 2;
    public int woodCount = 4;
    public int woodForHouse = 1;


    void Awake() {
        if (GameManager.instance) {
            Destroy(this);
        }
        else {
            GameManager.instance = this;
        }
        rain.SetActive(false);
    }

    private void Start() {
        DayNightCycle.instance.Nightfall += () => {
            foodCount -= population;
        };
    }

    private void Update() {
        waterPlane.position = Vector3.up * waterLevel;
        waterLevel = Mathf.Clamp(waterLevel, minWater, maxWater);

        if (rain.activeSelf) {
            waterLevel += rainIncrease * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine("Rain");
        }

        if (foodCount < 0) {
            foodCount = 0;
        }
        if (woodCount < 0) {
            woodCount = 0;
        }
    }

    public IEnumerator Rain () {
        rain.SetActive(true);
        SoundManager.instance.RainSound(true);
        yield return new WaitForSeconds(10);
        SoundManager.instance.RainSound(false);
        rain.SetActive(false);
    }

    public GameObject GetRandomTree () {
        int r = Random.Range(0, trees.Length);
        return trees[r];
    }
}