using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour {
    public LayerMask layerMask;
    public Transform mainCameraTransform;
    public Image reticleImage;
    public Sprite regularSprite;
    public Sprite interactableSprite;

    private readonly RaycastHit[] raycastResults = new RaycastHit[1];
    private const float RAYCAST_LENGTH = 3;
    void Update() {
        reticleImage.sprite = regularSprite;
        //Debug.DrawRay(mainCameraTransform.position, mainCameraTransform.forward, Color.red, 1000);
        Ray ray = new Ray(mainCameraTransform.position, mainCameraTransform.forward);
        int hitCount = Physics.RaycastNonAlloc(ray, raycastResults, RAYCAST_LENGTH, layerMask);
        if (hitCount > 0) {
            InteractableObject io = GetInteractableObject(raycastResults[0].transform.gameObject);
            if(io != null && io.interactable) {
                reticleImage.sprite = interactableSprite;
                if (InputUtil.Interact.WasPressed) {
                    io.Interact();
                }
            }
        }
    }

    private static InteractableObject GetInteractableObject(GameObject go) {
        if (go.TryGetComponent(out InteractableObject io)) {
            return io;
        }
        io = go.GetComponentInChildren<InteractableObject>();
        if (io != null) {
            return io;
        }
        io = go.GetComponentInParent<InteractableObject>();
        if (io != null) {
            return io;
        }
        return null;
    }
}
