using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInCanvas : MonoBehaviour {
    public Image mainImage;

    private IEnumerator Start() {
        Color startColor = mainImage.color;
        Color endColor = startColor.SetA(0);
        yield return new WaitForSeconds(0.7f);
        this.CreateUIAnimRoutine(1.2f, (float progress) => {
            float easedProgress = Easing.easeInOutSine(0f, 1f, progress);
            mainImage.color = Color.Lerp(startColor, endColor, easedProgress);
        }, () => {
            Destroy(gameObject);
        });
    }
}
