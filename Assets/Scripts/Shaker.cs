using UnityEngine;

public class Shaker : MonoBehaviour {
    public Transform mainT;
    public float speed;
    public Vector3 magnitude;

    private Vector3 startPos;
    private void Awake() {
        startPos = mainT.localPosition;
    }

    private void Update() {
        float time = Time.time * speed;
        float x = (Mathf.Sin(time) + Mathf.Sin(time * 2));
        float y = (Mathf.Sin(time * 0.9f) + Mathf.Sin(time * 2.2f));
        float z = (Mathf.Sin(time * 0.85f) + Mathf.Sin(time * 2.3f));
        mainT.localPosition = startPos + new Vector3(x * magnitude.x, y * magnitude.y, z * magnitude.z);
    }
}