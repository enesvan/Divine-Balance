using UnityEngine;

public class HousePlaceholder : MonoBehaviour {
    
    public void Build() {
        Instantiate(GameManager.instance.house, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
