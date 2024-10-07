using TMPro;
using UnityEngine;

public class KingGnomeFinish : InteractableObject {
    public SpriteRenderer mainRenderer;
    public AudioSource audioSource;
    public TMP_Text finishTimeText;
    public Collider mainCollider;
    protected override void DoInteraction() {
        mainRenderer.enabled = false;
        mainCollider.enabled = false;
        finishTimeText.text = Player.instance.RecordFinishTime().ToString("0.00");
        finishTimeText.gameObject.SetActive(true);
        audioSource.Play();
    }
}
