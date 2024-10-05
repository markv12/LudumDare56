using System;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour {
    [NonSerialized] public bool interactable = true;
    public void Interact() {
        if (interactable) {
            DoInteraction();
        }
    }

    protected abstract void DoInteraction();
}
