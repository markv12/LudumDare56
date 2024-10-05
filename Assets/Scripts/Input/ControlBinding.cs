using System.Collections.Generic;
using System.Text;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ControlBinding {
    public Key[] keys;
    public string[] inputPaths = new string[0];
    public bool forceOn = false;
    public void SetInputPaths(string[] newPaths) {
        inputPaths = newPaths;
        RefreshButtonControls();
    }

    public void RefreshButtonControls() {
        buttonControls.Clear();
        foreach (string inputPath in inputPaths) {
            foreach (InputDevice device in InputSystem.devices) {
                InputControl inputControl = InputControlPath.TryFindChild(device, inputPath);
                if (inputControl is ButtonControl buttonControl) {
                    buttonControls.Add(buttonControl);
                }
            }
        }
    }

    private readonly List<ButtonControl> buttonControls = new List<ButtonControl>();
    private readonly Key[] originalKeys;

    private readonly string id;
    public bool shift;
    private readonly bool originalShift;

    public bool control;
    private readonly bool originalControl;

    public bool alt;
    private readonly bool originalAlt;
    public ControlBinding() { }
    public ControlBinding(Key key, string _locID, bool _shift = false, bool _control = false, bool _alt = false) : this(new Key[] { key }, _locID, _shift, _control, _alt) { }

    public ControlBinding(Key[] _keys, string _id, bool _shift = false, bool _control = false, bool _alt = false) {
        keys = _keys;
        originalKeys = keys;

        id = _id;

        shift = _shift;
        originalShift = shift;

        control = _control;
        originalControl = control;

        alt = _alt;
        originalAlt = alt;
    }

    private static readonly StringBuilder sb = new StringBuilder(16);
    public string BindingsText {
        get {
            sb.Clear();
            for (int i = 0; i < keys.Length; i++) {
                Key key = keys[i];
                string keyString;
                if (key == Key.LeftBracket) { keyString = "["; } else if (key == Key.RightBracket) { keyString = "]"; } else { keyString = key.ToString(); }
                sb.Append(keyString);
                sb.Append(" ");
            }
            for (int i = 0; i < inputPaths.Length; i++) {
                sb.Append(inputPaths[i]);
                sb.Append(" ");
            }
            if (sb.Length > 0) {
                sb.Length--;
            }
            return sb.ToString();
        }
    }

    public bool WasPressed {
        get {
            if (forceOn) { return true; }
            if (CheckHoldKeys()) {
                if (Keyboard.current != null) {
                    for (int i = 0; i < keys.Length; i++) {
                        if (Keyboard.current[keys[i]].wasPressedThisFrame) {
                            return true;
                        }
                    }
                }
                for (int i = 0; i < buttonControls.Count; i++) {
                    if (buttonControls[i].wasPressedThisFrame) {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public bool IsPressed {
        get {
            if (forceOn) { return true; }
            if (CheckHoldKeys()) {
                if (Keyboard.current != null) {
                    for (int i = 0; i < keys.Length; i++) {
                        if (Keyboard.current[keys[i]].isPressed) {
                            return true;
                        }
                    }
                }
                for (int i = 0; i < buttonControls.Count; i++) {
                    if (buttonControls[i].isPressed) {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public bool WasReleased {
        get {
            if (Keyboard.current != null) {
                for (int i = 0; i < keys.Length; i++) {
                    if (Keyboard.current[keys[i]].wasReleasedThisFrame) {
                        return true;
                    }
                }
            }
            for (int i = 0; i < buttonControls.Count; i++) {
                if (buttonControls[i].wasReleasedThisFrame) {
                    return true;
                }
            }
            return false;
        }
    }

    private bool CheckHoldKeys() {
        Keyboard kc = Keyboard.current;
        if (shift && !kc.shiftKey.isPressed) { return false; }
        if (control && !(kc.ctrlKey.isPressed || kc.leftCommandKey.isPressed || kc.rightCommandKey.isPressed)) { return false; }
        if (alt && !kc.altKey.isPressed) { return false; }
        return true;
    }
}
