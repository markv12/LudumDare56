using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class TouchScreenMoveAndLook : MonoBehaviour {
    private int leftFingerId = -1;
    private int rightFingerId = -1;
    private float halfScreenWidth;

    private Vector2 moveTouchStartPosition;
    [NonSerialized] public Vector2 moveInput;
    [NonSerialized] public Vector2 lookInput;

    void Awake() {
        halfScreenWidth = Screen.width / 2;
    }

    public bool HasMoveInput => leftFingerId != -1;
    public bool HasLookInput => rightFingerId != -1;

    private void Update() {
        Touchscreen ts = Touchscreen.current;
        if (ts != null) {
            for (int i = 0; i < ts.touches.Count; i++) {
                TouchControl t = ts.touches[i];
                switch (t.phase.value) {
                    case UnityEngine.InputSystem.TouchPhase.Began:
                        if (t.position.value.x < halfScreenWidth && leftFingerId == -1) {
                            leftFingerId = t.touchId.value;
                            moveTouchStartPosition = t.position.value;
                        } else if (t.position.value.x > halfScreenWidth && rightFingerId == -1) {
                            rightFingerId = t.touchId.value;
                        }
                        break;
                    case UnityEngine.InputSystem.TouchPhase.Ended:
                    case UnityEngine.InputSystem.TouchPhase.Canceled:
                        if (t.touchId.value == leftFingerId) {
                            leftFingerId = -1;
                            moveInput = Vector2.zero;
                        } else if (t.touchId.value == rightFingerId) {
                            rightFingerId = -1;
                        }
                        break;
                    case UnityEngine.InputSystem.TouchPhase.Moved:
                        if (t.touchId.value == rightFingerId) {
                            lookInput = t.delta.value;
                        } else if (t.touchId.value == leftFingerId) {
                            moveInput = t.position.value - moveTouchStartPosition;
                        }
                        break;
                    case UnityEngine.InputSystem.TouchPhase.Stationary:
                        if (t.touchId.value == rightFingerId) {
                            lookInput = Vector2.zero;
                        }
                        break;
                }
            }
        }
    }

    public void CancelDrag() {
        leftFingerId = -1;
        rightFingerId = -1;
        moveInput = Vector2.zero;
        lookInput = Vector2.zero;
    }
}
