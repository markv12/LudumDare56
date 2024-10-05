using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityStandardAssets.Characters.FirstPerson {
    [Serializable]
    public class MouseLook {
        private Transform character;
        private Transform camera;

        public float XSensitivity;
        public float YSensitivity;
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;
        public bool lockCursor = true;
        public TouchScreenMoveAndLook moveAndLook;

        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;
        private bool m_cursorIsLocked = true;

        public void Init(Transform _character, Transform _camera) {
            character = _character;
            camera = _camera;
            ResetTargetRotations();
        }

        public void ResetTargetRotations() {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }

        public void LookRotation() {
            float speedSetting = 0.8f;
            if (InputUtil.IsMobile) {
                if (moveAndLook.HasLookInput) {
                    float xRot = moveAndLook.lookInput.y * XSensitivity * speedSetting;
                    float yRot = moveAndLook.lookInput.x * YSensitivity * speedSetting;
                    ApplyRotationToCharacter(xRot, yRot);
                }
            } else {
                float xRot = Mouse.current.delta.y.ReadValue() * XSensitivity * speedSetting;
                float yRot = Mouse.current.delta.x.ReadValue() * YSensitivity * speedSetting;
                ApplyRotationToCharacter(xRot, yRot);
                UpdateCursorLock();
            }
        }

        private void ApplyRotationToCharacter(float xRot, float yRot) {
            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

            if (smooth) {
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            } else {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }
        }

        public void SetCursorLock(bool value) {
            lockCursor = value;
            if (!lockCursor) {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void UpdateCursorLock() {
            //if the user set "lockCursor" we check & properly lock the cursor
            if (lockCursor)
                InternalLockUpdate();
        }

        private void InternalLockUpdate() {
            if (InputUtil.Cancel.WasPressed) {
                m_cursorIsLocked = false;
            } else if (Mouse.current.leftButton.wasReleasedThisFrame) {
                m_cursorIsLocked = true;
            }

            if (m_cursorIsLocked) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            } else if (!m_cursorIsLocked) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q) {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
    }
}
