using UnityEngine;

public class DisappearAtDistasnce : MonoBehaviour {
    public Transform mainT;
    private void Update() {
        if (Player.instance != null && (Player.instance.t.position - mainT.position).sqrMagnitude < 50) {
            Destroy(gameObject);
        }
    }
}
