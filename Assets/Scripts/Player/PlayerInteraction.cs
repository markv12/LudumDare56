using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour {
    public LayerMask layerMask;
    public Transform mainCameraTransform;
    public Image reticleImage;
    public Sprite regularSprite;
    public Sprite interactableSprite;

    private readonly RaycastHit[] raycastResults = new RaycastHit[1];
    void Update() {
        reticleImage.sprite = regularSprite;
        //Debug.DrawRay(mainCameraTransform.position, mainCameraTransform.forward, Color.red, 1000);
        Ray ray = new Ray(mainCameraTransform.position, mainCameraTransform.forward);
        if (Physics.RaycastNonAlloc(ray, raycastResults, 5, layerMask) > 0) {
            reticleImage.sprite = interactableSprite;
        }

        if (InputUtil.Interact.WasPressed) {
            if (Physics.RaycastNonAlloc(ray, raycastResults, 5, layerMask) > 0) {
                TryToInteract(raycastResults[0].transform.gameObject);
            }
        }
    }

    private static void TryToInteract(GameObject go) {
        if (go.TryGetComponent(out InteractableObject io)) {
            io.Interact();
            return;
        } else {
            io = go.GetComponentInChildren<InteractableObject>();
            if (io != null) {
                io.Interact();
            } else {
                io = go.GetComponentInParent<InteractableObject>();
                if (io != null) {
                    io.Interact();
                }
            }
        }
    }
}
