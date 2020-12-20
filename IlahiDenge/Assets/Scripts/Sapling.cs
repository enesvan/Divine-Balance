using UnityEngine;
using System.Collections;

public class Sapling : MonoBehaviour {

    public float growSeconds = 30;
    public float drainSpeed = 0.5f;

    GameManager gm;

    private void Awake() {
    }

    private void Start() {
        gm = GameManager.instance;
        StartCoroutine("Grow");
    }

    private void Update() {
        gm.waterLevel -= drainSpeed * Time.deltaTime;
    }

    IEnumerator Grow() {
        yield return new WaitForSeconds(growSeconds);

        Instantiate(gm.GetRandomTree(), transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

}
