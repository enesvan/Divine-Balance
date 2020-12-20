using UnityEngine;

public class HumanStats : MonoBehaviour
{
    public float maxHunger, minFaith;
    public float hungerRate, basicFaithDecreaseRate, faithIncreaseRate, faithDecreaseRate;
    public float hunger, faith;
    void Start()
    {

    }

    void Update()
    {
        if (hunger < maxHunger)
            hunger += hungerRate * Time.deltaTime;

        if (hunger >= maxHunger)
            Die();

        if (faith > minFaith)
            faith -= basicFaithDecreaseRate * Time.deltaTime;

        if (faith <= minFaith)
            Suicide();
    }

    public void Die()
    {
        // Destroy(Human)
    }

    public void Suicide()
    {
        // There is a small alert shows on the screen like "A human killed himself for faith"
        // Destroy(Human)
    }
}