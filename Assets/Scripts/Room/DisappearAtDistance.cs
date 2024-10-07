using UnityEngine;

public class DisappearAtDistasnce : MonoBehaviour {
    public Transform mainT;
    public SpriteRenderer mainRenderer;

    private bool disappeared = false;
    private void Update() {
        if (!disappeared && Player.instance != null && (Player.instance.t.position - mainT.position).sqrMagnitude < 50) {
            disappeared = true;
            Color startColor = mainRenderer.color;
            Color endColor = startColor.SetA(0);
            this.CreateWorldAnimRoutine(1f, (float progress) => {
                mainRenderer.color = Color.Lerp(startColor, endColor, Easing.easeOutSine(0, 1, progress));
            }, () => {
                Destroy(gameObject);
            });
        }
    }
}
