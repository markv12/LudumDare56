using System.Collections;
using UnityEngine;

public class WorldSpriteLoopTwoSided : MonoBehaviour {
    public SpriteRenderer frontRenderer;
    public SpriteRenderer backRenderer;
    public Sprite[] frontSprites;
    public Sprite[] backSprites;
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
            for (int i = 0; i < frontSprites.Length; i++) {
                frontRenderer.sprite = frontSprites[i];
                backRenderer.sprite = backSprites[i];
                float elapsedTime = 0;
                while (elapsedTime < timePerFrame) {
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
}
