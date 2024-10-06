using UnityEngine;

public class Spinner : MonoBehaviour {
    public Transform mainT;
    public Vector3 spinSpeed;
    void Update() {
        mainT.Rotate(spinSpeed * Time.deltaTime);
    }
}
