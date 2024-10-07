using UnityEngine;

public class Door : InteractableObject {
    public Transform mainT;
    public Collider mainCollider;
    public Transform hingeT;
    public Vector3 closedRotation;
    private Quaternion closedQuaternion;
    public Vector3 openRotation;
    private Quaternion openQuaternion;
    public const float DOOR_CLOSE_TIME = 0.6f;
    public AudioSource doorSound;
    private bool isOpen = false;

    private void Awake() {
        RefreshQuaternions();
    }

    public void SetOpenRotation(Vector3 _openRotation) {
        openRotation = _openRotation;
        RefreshQuaternions();
    }

    private void RefreshQuaternions() {
        openQuaternion = Quaternion.Euler(openRotation);
        closedQuaternion = Quaternion.Euler(closedRotation);
    }

    protected override void DoInteraction() {
        isOpen = !isOpen;
        Animate(isOpen);
    }

    private Coroutine animateRoutine = null;
    private void Animate(bool isOpen) {
        //mainCollider.enabled = !isOpen;
        Quaternion startRotation = hingeT.localRotation;
        Quaternion endRotation = isOpen ? openQuaternion : closedQuaternion;
        this.EnsureCoroutineStopped(ref animateRoutine);
        animateRoutine = this.CreateWorldAnimRoutine(DOOR_CLOSE_TIME, (float progress) => {
            hingeT.localRotation = Quaternion.Lerp(startRotation, endRotation, Easing.easeInOutSine(0, 1, progress));
        });
        if(doorSound != null) {
            doorSound.volume = SettingsManager.SFXVolume;
            doorSound.Play();
        }
    }

    public void Close() {
        Animate(false);
    }
}
