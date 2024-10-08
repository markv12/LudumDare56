using UnityEngine;

public class BobUpAndDown : MonoBehaviour {
    public float amplitude = 0.1f;
    public float frequency = 0.3f;
    private float randomOffset = 0;
    float startY;

    void Start() {
        startY = transform.localPosition.y;
        randomOffset = Random.Range(0f, 5f);
    }

    void Update() {
        float y = startY + Mathf.Sin(Time.fixedTime * Mathf.PI * frequency + randomOffset) * amplitude;
        transform.localPosition = transform.localPosition.SetY(y);
    }
}
