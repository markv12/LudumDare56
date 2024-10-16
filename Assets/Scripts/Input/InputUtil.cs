using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class InputUtil : MonoBehaviour {

    public static readonly ControlBinding Interact = new ControlBinding(Key.E, "interact");
    public static readonly ControlBinding Cancel = new ControlBinding(Key.Escape, "cancel");
    public static readonly ControlBinding Jump = new ControlBinding(Key.Space, "jump");
    public static readonly ControlBinding ToggleRun = new ControlBinding(new Key[] { }, "togglerun");
    public static readonly ControlBinding Crouch = new ControlBinding(new Key[] {
#if !UNITY_WEBGL
        Key.LeftCtrl,
        Key.RightCtrl,
#endif
        Key.LeftShift,
        Key.RightShift,
        Key.C,
    }, "crouch");
    public static readonly ControlBinding PauseMenu = new ControlBinding(Key.P, "pausemenu");
    public static readonly ControlBinding Gun = new ControlBinding(Key.G, "gun");
    public static readonly ControlBinding ThrowGrenade = new ControlBinding(Key.V, "throwgrenade");

    public static readonly ControlBinding Undo = new ControlBinding(Key.Z, "undo", _control: true);
    public static readonly ControlBinding Redo = new ControlBinding(Key.Z, "redo", _control: true, _shift: true);
    public static readonly ControlBinding Redo2 = new ControlBinding(Key.Y, "redo", _control: true);
    public static readonly ControlBinding FlipCanvas = new ControlBinding(Key.F, "flipcanvas");
    public static readonly ControlBinding GoToBrush = new ControlBinding(Key.B, "gotobrush");
    public static readonly ControlBinding GoToPaintBucket = new ControlBinding(Key.G, "gotopaintbucket");
    public static readonly ControlBinding InvertPattern = new ControlBinding(Key.Y, "invertpattern");
    public static readonly ControlBinding NextPattern = new ControlBinding(Key.R, "nextpattern");
    public static readonly ControlBinding PrevPattern = new ControlBinding(Key.R, "prevpattern", _shift: true);
    public static readonly ControlBinding ResetPattern = new ControlBinding(Key.T, "resetpattern");
    public static readonly ControlBinding ShapeFill = new ControlBinding(Key.Q, "shapefill");
    public static readonly ControlBinding ToggleGlobalFill = new ControlBinding(Key.H, "toggleglobalfill");
    public static readonly ControlBinding BrushSizeUp = new ControlBinding(Key.RightBracket, "brushsizeup");
    public static readonly ControlBinding BrushSizeDown = new ControlBinding(Key.LeftBracket, "brushsizedown");
    public static readonly ControlBinding SmoothUp = new ControlBinding(Key.RightBracket, "smoothup", _shift: true);
    public static readonly ControlBinding SmoothDown = new ControlBinding(Key.LeftBracket, "smoothdown", _shift: true);
    public static readonly ControlBinding ZoomIn = new ControlBinding(Key.RightBracket, "zoomin", _alt: true);
    public static readonly ControlBinding ZoomOut = new ControlBinding(Key.LeftBracket, "zoomout", _alt: true);
    public static readonly ControlBinding ShowPreviousTexture = new ControlBinding(Key.C, "showprevtexture");
    public static readonly ControlBinding DrawStraightLine = new ControlBinding(new Key[] { Key.LeftShift, Key.RightShift }, "drawstraightline");
    public static readonly ControlBinding Black = new ControlBinding(new Key[] { Key.Digit1, Key.Numpad1 }, "black");
    public static readonly ControlBinding White = new ControlBinding(new Key[] { Key.Digit2, Key.Numpad2 }, "white");
    public static readonly ControlBinding Grey = new ControlBinding(new Key[] { Key.Digit3, Key.Numpad3 }, "grey");
    public static readonly ControlBinding Red = new ControlBinding(new Key[] { Key.Digit4, Key.Numpad4 }, "red");
    public static readonly ControlBinding Yellow = new ControlBinding(new Key[] { Key.Digit5, Key.Numpad5 }, "yellow");
    public static readonly ControlBinding Green = new ControlBinding(new Key[] { Key.Digit6, Key.Numpad6 }, "green");
    public static readonly ControlBinding Blue = new ControlBinding(new Key[] { Key.Digit7, Key.Numpad7 }, "blue");
    public static readonly ControlBinding Purple = new ControlBinding(new Key[] { Key.Digit8, Key.Numpad8 }, "purple");
    public static readonly ControlBinding Pink = new ControlBinding(new Key[] { Key.Digit9, Key.Numpad9 }, "pink");

    public static readonly ControlBinding UISelect = new ControlBinding(new Key[] { Key.E, Key.Space }, "uiselect");
    public static readonly ControlBinding Up = new ControlBinding(new Key[] { Key.W, Key.UpArrow }, "up");
    public static readonly ControlBinding Down = new ControlBinding(new Key[] { Key.S, Key.DownArrow }, "down");
    public static readonly ControlBinding Left = new ControlBinding(new Key[] { Key.A, Key.LeftArrow }, "left");
    public static readonly ControlBinding Right = new ControlBinding(new Key[] { Key.D, Key.RightArrow }, "right");
    public static readonly ControlBinding UITab = new ControlBinding(Key.Tab, "uitab");
    public static readonly ControlBinding UIShift = new ControlBinding(new Key[] { Key.LeftShift, Key.RightShift }, "uishift");

    public static readonly ControlBinding[] allControls = new ControlBinding[] {
        Interact,
        Cancel,
        Jump,
        ToggleRun,
        Crouch,
        PauseMenu,
        Undo,
        Redo,
        Redo2,
        FlipCanvas,
        GoToBrush,
        GoToPaintBucket,
        InvertPattern,
        NextPattern,
        PrevPattern,
        ResetPattern,
        ShapeFill,
        ToggleGlobalFill,
        BrushSizeUp,
        BrushSizeDown,
        SmoothUp,
        SmoothDown,
        ZoomIn,
        ZoomOut,
        ShowPreviousTexture,
        DrawStraightLine,
        Black,
        White,
        Grey,
        Red,
        Yellow,
        Green,
        Blue,
        Purple,
        Pink,
        //UISelect,
        //Up,
        //Down,
        //Left,
        //Right,
        Gun,
        ThrowGrenade,
    };

    public static readonly string[] unbindableControlLocIDs = new string[] {
        "mouselook",
        "wasdarrowsmove",
        "rightclickeyedropper",
        "scrollbrushsize",
        "scrollsmoothingsize",
        "scrollzoom",
    };
    public static float GetHorizontal() {
        float result = 0;
        if (Left.IsPressed) {
            result -= 1;
        }
        if (Right.IsPressed) {
            result += 1;
        }
        return result;
    }

    public static float GetVertical() {
        float result = 0;
        if (Down.IsPressed) {
            result -= 1;
        }
        if (Up.IsPressed) {
            result += 1;
        }
        return result;
    }

    public static bool GetKeyDown(Key key) {
        return Keyboard.current != null && Keyboard.current[key].wasPressedThisFrame;
    }

    public static bool GetKey(Key key) {
        return Keyboard.current != null && Keyboard.current[key].isPressed;
    }

    public static bool GetKeyUp(Key key) {
        return Keyboard.current != null && Keyboard.current[key].wasReleasedThisFrame;
    }

    public static float MouseScrollDelta => Mouse.current == null ? 0 : Mouse.current.scroll.ReadValue().y;

    public static Vector2 MousePosition {
        get {
            if (Pen.current != null && Pen.current.inRange.ReadValue() > 0.5f) {
                return Pen.current.position.ReadValue();
            } else if (HasMobileTouch) {
                return GetTouchCenterPosition();
            } else {
                return Mouse.current == null ? Vector2.zero : Mouse.current.position.ReadValue();
            }
        }
    }

    public static bool LeftMouseButtonDown => buttonDownFrame == Time.frameCount;
    public static bool LeftMouseButtonUp => buttonUpFrame == Time.frameCount;
    public static bool LeftMouseButtonIsPressed {
        get {
            return (Mouse.current != null && Mouse.current.leftButton.isPressed) || ReadPen > 0.001f || HasMobileTouch;
        }
    }
    public static bool RightMouseButtonDown => Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame;
    private static bool HasMobileTouch {
        get {
            if (IsMobile) {
                if (Touchscreen.current != null) {
                    for (int i = 0; i < Touchscreen.current.touches.Count; i++) {
                        TouchControl touch = Touchscreen.current.touches[i];
                        if (touch.IsPressed()) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }

    private static float ReadPen => Pen.current == null ? 0f : Pen.current.pressure.ReadValue();

    private static int buttonDownFrame;
    private static int buttonUpFrame;
    private static bool prevIsPressed;

    private static float currentPenPressure;
    private static float prevPenRead;
    private void Update() {
        bool isPressed = LeftMouseButtonIsPressed;
        if (isPressed && !prevIsPressed) {
            buttonDownFrame = Time.frameCount;
        } else if (!isPressed && prevIsPressed) {
            buttonUpFrame = Time.frameCount;
        }
        prevIsPressed = isPressed;
        float penRead = ReadPen;
        currentPenPressure = penRead;
        if (prevPenRead > 0 && penRead <= 0) {
            StartCoroutine(ResetRoutine());
        }
        prevPenRead = penRead;
    }

    private IEnumerator ResetRoutine() {
        yield return null;
        yield return null;
        yield return null;
        InputSystem.ResetDevice(Mouse.current, true);
    }

    private static Vector2 GetTouchCenterPosition() {
        if (Touchscreen.current != null) {
            int touchCount = 0;
            Vector2 accum = Vector2.zero;
            foreach (TouchControl touch in Touchscreen.current.touches) {
                if (touch.IsPressed()) {
                    touchCount++;
                    accum += touch.position.ReadValue();
                }
            }
            if (touchCount > 0) {
                return accum / touchCount;
            }
        }
        return Vector2.zero;
    }

    public static bool CTRLPressed => GetKey(Key.LeftCtrl) || GetKey(Key.RightCtrl) || GetKey(Key.LeftCommand) || GetKey(Key.RightCommand);
    public static bool ShiftPressed => GetKey(Key.LeftShift) || GetKey(Key.RightShift);
    public static bool AltPressed => GetKey(Key.LeftAlt) || GetKey(Key.LeftAlt);

    public static bool ScrollWheelUp => MouseScrollDelta < -0.001f;
    public static bool ScrollWheelDown => MouseScrollDelta > 0.001f;

    public static bool IsMobile => UnityEngine.Device.Application.isMobilePlatform;
}

public static class TouchControlExtensions {
    public static bool IsActive(this TouchControl touchControl) {
        TouchPhase touchPhase = touchControl.phase.value;
        return touchPhase == TouchPhase.Began || touchPhase == TouchPhase.Stationary || touchPhase == TouchPhase.Moved;
    }

    public static bool IsPressed(this TouchControl touchControl) {
        return touchControl.press.isPressed && touchControl.IsActive();
    }
}
