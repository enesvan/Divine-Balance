using UnityEngine;
using System.Collections;

public class Dodo : AI {

    public GameObject stdModel;
    public float eatSpeed = 0.2f;
    public float deathTime = 60;

    Animator stdAnim;
    GameManager gm;
    SoundManager sm;
    Rigidbody rb;
    Plant plantTarget;
    bool isDead;

    void Start() {
        gm = GameManager.instance;
        sm = SoundManager.instance;

        stdAnim = stdModel.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        

        DayNightCycle.instance.Nightfall += () => {
            StopAllCoroutines();
            FindFood();
        };
        DayNightCycle.instance.Daylight += () => {
            StartCoroutine("Idle");
        };
        


        if (DayNightCycle.instance.isNight) {
            FindFood();
        }
        else {
            StartCoroutine("Idle");
        }

        gm.dodoPopulation++;
        Invoke("BunuBanaZorlaYazdirdilar", deathTime);
    }

    private void Update() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (isDead) return;

        if (plantTarget) {
            plantTarget.health -= Time.deltaTime * eatSpeed;
            plantTarget.vol = sm.sfxSound;
        }
    }

    void FindFood () {
        if (isDead) return;
        
        GameObject bush = GameObject.FindGameObjectWithTag("bush");
        if (!bush) {
            return;
        }

        bush.GetComponent<Plant>().OnDeath += GoIdle;

        currentState = AIState.Walking;

        agent.SetDestination(bush.transform.position);
    }

    void GoIdle () {
        if (isDead) return;
        plantTarget = null;
        StartCoroutine("Idle");
    }

    void Eat () {
        stdAnim.SetBool("eating", true);
    }

    IEnumerator Idle () {

        if (!isDead) {
            currentState = AIState.Idle;

            stdAnim.SetBool("eating", false);
            if (plantTarget) {
                plantTarget.vol = 0;
                plantTarget = null;
            }
            Vector3 rPoint = new Vector3(Random.Range(-155, 155), 0, Random.Range(-155, 155));
            agent.SetDestination(transform.position + rPoint);
        }
        
        

        

        yield return new WaitForSeconds(2);
        StartCoroutine("Idle");
    }

    void OnCollisionEnter(Collision col) {
        if (col.transform.tag == "bush" && currentState == AIState.Walking) {
            plantTarget = col.transform.GetComponent<Plant>();
            Eat();
        }
    }

    void BunuBanaZorlaYazdirdilar () {
        isDead = true;
        transform.position = Vector3.up * -99;
        gm.dodoPopulation--;

        Destroy(agent);
    }
}
