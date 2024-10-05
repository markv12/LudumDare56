using System;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public static class SettingsManager {

    public static event Action musicVolumeChanged;
    private const string MUSIC_VOLUME = "music_volume";
    public static float MusicVolume {
        get {

            if (!PlayerPrefs.HasKey(MUSIC_VOLUME)) {
                PlayerPrefs.SetFloat(MUSIC_VOLUME, 1f);
            }
            return PlayerPrefs.GetFloat(MUSIC_VOLUME);
        }
        set {
            PlayerPrefs.SetFloat(MUSIC_VOLUME, Mathf.Min(1f, Mathf.Max(0f, value)));
            musicVolumeChanged?.Invoke();
        }
    }

    private const string SFX_VOLUME = "sfx_volume";
    public static float SFXVolume {
        get {

            if (!PlayerPrefs.HasKey(SFX_VOLUME)) {
                PlayerPrefs.SetFloat(SFX_VOLUME, 1f);
            }
            return PlayerPrefs.GetFloat(SFX_VOLUME);
        }
        set {
            PlayerPrefs.SetFloat(SFX_VOLUME, Mathf.Min(1f, Mathf.Max(0f, value)));
        }
    }

    private const string LOOK_SPEED = "look_speed";
    public static float LookSpeed {
        get {

            if (!PlayerPrefs.HasKey(LOOK_SPEED)) {
                PlayerPrefs.SetFloat(LOOK_SPEED, SettingsUI.MAX_LOOK_SPEED / 2f);
            }
            return PlayerPrefs.GetFloat(LOOK_SPEED);
        }
        set {
            PlayerPrefs.SetFloat(LOOK_SPEED, Mathf.Min(2f, Mathf.Max(0f, value)));
        }
    }

    private const string VSYNC_ENABLED = "vsync_enabled";
    public static bool VSyncEnabled {
        get {
            return PlayerPrefs.GetString(VSYNC_ENABLED, "true") == "true";
        }
        set {
            PlayerPrefs.SetString(VSYNC_ENABLED, value ? "true" : "false");
        }
    }

    private const string ANTI_ALIASING_ENABLED = "anti_aliasing_enabled";
    public static bool AntiAliasingEnabled {
        get {
            return PlayerPrefs.GetString(ANTI_ALIASING_ENABLED, "true") == "true";
        }
        set {
            PlayerPrefs.SetString(ANTI_ALIASING_ENABLED, value ? "true" : "false");
        }
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Clear Player Prefs")]
    public static void Clear() {
        PlayerPrefs.DeleteAll();
    }
#endif
}
