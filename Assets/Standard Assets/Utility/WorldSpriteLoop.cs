using System.Collections;
using UnityEngine;

public class WorldSpriteLoop : MonoBehaviour {
    public SpriteRenderer[] mainRenderers;
    public Sprite[] sprites;
    public float frameRate;

    private Coroutine animateRoutine = null;
    private void OnEnable() {
        this.EnsureCoroutineStopped(ref animateRoutine);
        animateRoutine = StartCoroutine(Co_Animate());
    }

    IEnumerator Co_Animate() {
        yield return null;
        yield return null;
        yield return null;
        float timePerFrame = 1f / frameRate;
        float waitTime = Random.Range(0f, timePerFrame);
        yield return new WaitForSeconds(waitTime);

        while (true) {
            for (int i = 0; i < sprites.Length; i++) {
                for (int j = 0; j < mainRenderers.Length; j++) {
                    mainRenderers[j].sprite = sprites[i];
                }
                float elapsedTime = 0;
                while (elapsedTime < timePerFrame) {
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
}
