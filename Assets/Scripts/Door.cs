using UnityEngine;

public class Door : InteractableObject {
    public Collider mainCollider;
    public Transform hingeT;

    private bool isOpen = false;

    protected override void DoInteraction() {
        isOpen = !isOpen;
        Animate(isOpen);
    }

    private static readonly Quaternion closedRotation = Quaternion.identity;
    private static readonly Quaternion openRotation = Quaternion.Euler(0, 0, -110);
    private Coroutine animateRoutine = null;
    private void Animate(bool isOpen) {
        //mainCollider.enabled = !isOpen;
        Quaternion startRotation = hingeT.localRotation;
        Quaternion endRotation = isOpen ? openRotation : closedRotation;
        this.EnsureCoroutineStopped(ref animateRoutine);
        animateRoutine = this.CreateWorldAnimRoutine(1f, (float progress) => {
            hingeT.localRotation = Quaternion.Lerp(startRotation, endRotation, Easing.easeInOutSine(0, 1, progress));
        });
    }
}
