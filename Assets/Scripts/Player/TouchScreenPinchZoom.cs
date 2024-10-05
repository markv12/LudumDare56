using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;
using System;
using UnityEngine;
using System.Collections.Generic;

public class TouchScreenPinchZoom : MonoBehaviour {
    [NonSerialized] public int zoomInput;
    private float lastDistance = -1;

    private readonly List<TouchControl> activeTouches = new List<TouchControl>();
    public int TouchCount => activeTouches.Count;
    private float lastPinchTime;
    public bool WasPinching {
        get { return InputUtil.IsMobile && Time.time - lastPinchTime < 0.5f; }
    }
    private void Update() {
        Touchscreen ts = Touchscreen.current;
        if (ts != null) {
            zoomInput = 0;
            activeTouches.Clear();
            foreach (TouchControl touch in ts.touches) {
                if(touch.IsActive()) {
                    activeTouches.Add(touch);
                }
            }
            if (activeTouches.Count >= 2) {
                lastPinchTime = Time.time;
                TouchControl touch1 = activeTouches[0];
                TouchControl touch2 = activeTouches[1];
                float touchDistance = (touch1.position.value - touch2.position.value).magnitude;
                if (lastDistance > 0) {
                    float distDiff = touchDistance - lastDistance;
                    if (distDiff > 50) {
                        zoomInput = 1;
                        lastDistance = touchDistance;
                    } else if (distDiff < -50) {
                        zoomInput = -1;
                        lastDistance = touchDistance;
                    }
                } else {
                    lastDistance = touchDistance;
                }
            } else {
                lastDistance = -1;
            }
        }
    }
}
