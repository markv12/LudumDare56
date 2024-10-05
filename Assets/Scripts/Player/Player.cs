using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour {
    public static Player instance;

    public Transform t;
    public Camera mainCamera;
    [NonSerialized] public Transform mainCameraTransform;

    public CharacterController characterController;
    public FirstPersonController firstPersonController;
    public SettingsUI settingsUI;

    public Vector3 FaceDirection => mainCameraTransform.forward;

    private Vector3 playerStartPos;
    private Quaternion playerStartRotation;

    private void Awake() {
        instance = this;
        mainCameraTransform = mainCamera.transform;
        playerStartPos = t.position;
        playerStartRotation = t.rotation;
    }

    private void Update() {
        if (InputUtil.Cancel.WasPressed || InputUtil.PauseMenu.WasPressed) {
            OpenSettingsUI();
        }
        if (Time.frameCount % 5 == 0 && t.position.y < -120) {
            ReturnToEntrance();
        }
    }

    private void SetFPSControllerActive(bool isActive) {
        enabled = isActive;
        characterController.enabled = isActive;
        firstPersonController.enabled = isActive;
        Cursor.lockState = isActive && !InputUtil.IsMobile ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isActive || InputUtil.IsMobile;
    }

    public void ReturnToEntrance() {
        StartCoroutine(ReturnRoutine());

        IEnumerator ReturnRoutine() {
            characterController.enabled = false;
            firstPersonController.enabled = false;
            yield return null;
            t.SetPositionAndRotation(playerStartPos, playerStartRotation);
            firstPersonController.ResetMouseLook();
            firstPersonController.ResetCameraRotation();
            yield return null;
            characterController.enabled = true;
            firstPersonController.enabled = true;
        }
    }

    public void OpenSettingsUI() {
        SetFPSControllerActive(false);
        settingsUI.Open(false, () => {
            SetFPSControllerActive(true);
        });
    }
}
