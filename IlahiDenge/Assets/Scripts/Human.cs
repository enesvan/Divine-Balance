using UnityEngine;
using System.Collections;
using Globals;

public class Human : AI {

    public enum Role {
        None, Lumberjack, Collector, Builder
    }

    public GameObject stdModel;
    public GameObject lumberModel;

    public House home;

    public Role currentRole = Role.Lumberjack;
    public LayerMask buildMask;


    public int foodGain = 2;

    int woodInv, foodInv;
    Animator stdAnim;
    GameManager gm;
    SoundManager sm;

    Transform placeholder;
    Plant plantTarget;
    Rigidbody rb;

    void Start() {
        gm = GameManager.instance;
        stdAnim = stdModel.GetComponent<Animator>();
        sm = SoundManager.instance;

        DayNightCycle.instance.Nightfall += GoHome;
        DayNightCycle.instance.Daylight += GoWork;
        DayNightCycle.instance.Daylight += Breed;

        
        rb = GetComponent<Rigidbody>();
        

        if (!DayNightCycle.instance.isNight) {
            GoWork();
        }


        ResetModel();
        
    }

    private void Update() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (plantTarget) {
            plantTarget.health -= Time.deltaTime;
            plantTarget.vol = sm.sfxSound;
        }
    }

    void ResetModel () {
        stdModel.SetActive(true);
        lumberModel.SetActive(false);
    }

    void Breed () {
        if (!home) return;
        int rng = Random.Range(0,10);
        
        if (home.population > 1 && gm.foodCount > gm.foodCountForBreed) {
            
            House childHome = null;

            foreach (var h in FindObjectsOfType<House>()) {
                if (h.population < h.capacity) {
                    childHome = h;
                    h.population++;
                }
            }

            if (childHome) {
                var p2spawn = Random.Range(0, 2) > 1 ? gm.male : gm.female;
                var human = Instantiate(p2spawn, childHome.transform.position, Quaternion.identity);
                human.GetComponent<Human>().home = childHome;
                gm.population++;
            }
            
        } 
    }

    void FindPlant (string tag) {
        var bush = GameObject.FindGameObjectWithTag(tag);

        if (!bush) {
            return;
        }

        bush.GetComponent<Plant>().OnDeath += () => {
            ResetPlantTarget();
            stdAnim.SetBool("collecting", false);
            ResetModel();
            StopAllCoroutines();
            currentRole = Role.None;

            if (!DayNightCycle.instance.isNight) {
                GoWork();
            }
            
        };

        currentState = AIState.Walking;
        
        agent.SetDestination(bush.transform.position);
    }

    void GoHome () {
        if (!home) {
            return;
        }
        currentState = AIState.Walking;

        agent.SetDestination(home.transform.position);
        StopAllCoroutines();
    }

    void CarryHome () {
        if (!home) {
            return;
        }
        currentState = AIState.Carrying;
        ResetModel();
        agent.SetDestination(home.transform.position);
    }

    void GoWork () {
        if (currentRole == Role.None && home)
            currentRole = (Role)Random.Range(1, 3);

        if (currentRole == Role.Lumberjack)
            FindPlant("tree");

        if (currentRole == Role.Collector)
            FindPlant("bush");

        if (currentRole == Role.Builder) {
            if (placeholder) {
                agent.SetDestination(placeholder.position);
            }
            else {
                MakeHouse();
            }
        }
            
        
        ResetModel();
    }

    IEnumerator DoWork (G.VoidDel callback) {
        currentState = AIState.Working;
        agent.SetDestination(transform.position);

        if (currentRole == Role.Lumberjack) {
            stdModel.SetActive(false);
            lumberModel.SetActive(true);
        }

        if (currentRole == Role.Collector) {
            stdAnim.SetBool("collecting", true);
        }

        yield return new WaitForSeconds(2);
        callback();
    }

    void MakeHouse () {
        currentRole = Role.Builder;
        currentState = AIState.Building;
        gm.woodCount-=gm.woodForHouse;

        int maxP = 50;

        Vector3 rPoint;
        Quaternion rRot;

        void TestCollision () {
            rPoint = new Vector3(Random.Range(0, maxP), 0, Random.Range(0, maxP));
            rRot = Quaternion.Euler(0, Random.Range(0, 360), 0);

            Collider[] hitColliders = Physics.OverlapBox(rPoint, Vector3.one * 5, rRot, buildMask);
            if (hitColliders.Length > 0) {
                TestCollision();
            }
        }

        TestCollision();
        placeholder = Instantiate(gm.housePlaceholder, rPoint, rRot).transform;
        agent.SetDestination(placeholder.position);
    }

    void ResetPlantTarget() {
        if (plantTarget) {
            plantTarget.vol = 0;
            plantTarget = null;
        }
    }

    void OnCollisionEnter(Collision col) {
        if (currentRole == Role.Lumberjack && col.transform.tag == "tree" && currentState == AIState.Walking) {
            plantTarget = col.transform.GetComponent<Plant>();

            StartCoroutine( DoWork(() => {
                ResetPlantTarget();
                woodInv++;
                CarryHome();
            }));
        }
        if (currentRole == Role.Collector && col.transform.tag == "bush" && currentState == AIState.Walking) {
            plantTarget = col.transform.GetComponent<Plant>();

            StartCoroutine(DoWork(() => {
                ResetPlantTarget();
                foodInv+=foodGain;
                stdAnim.SetBool("collecting", false);
                CarryHome();
            }));
        }

        
        if (col.transform.tag == "house" && currentState == AIState.Carrying) {
            gm.woodCount += woodInv;
            woodInv = 0;

            gm.foodCount += foodInv;
            foodInv = 0;

            if (gm.woodCount > gm.woodForHouse) {
                MakeHouse();
            }
            //GoWork();
        }

        var hp = col.transform.GetComponent<HousePlaceholder>();
        
        if (hp && currentRole == Role.Builder) {
            
            hp.Build();
            placeholder = null;

            currentRole = Role.None;
            GoWork();
        }
    }
}

