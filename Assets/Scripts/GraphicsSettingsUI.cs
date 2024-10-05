using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettingsUI : MonoBehaviour {
    public Button closeButton;

    public Checkbox fullScreenCheckbox;
    public Checkbox vsyncCheckbox;
    public Checkbox antiAliasingCheckbox;

    public TMP_Text resolutionText;
    public Button prevResolutionButton;
    public Button nextResolutionButton;

    void Awake() {
        fullScreenCheckbox.IsChecked = Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen || Screen.fullScreenMode == FullScreenMode.FullScreenWindow || Screen.fullScreenMode == FullScreenMode.MaximizedWindow;
        fullScreenCheckbox.onChange += FullScreenChanged;

        vsyncCheckbox.IsChecked = SettingsManager.VSyncEnabled;
        vsyncCheckbox.onChange += VSyncChanged;

        antiAliasingCheckbox.IsChecked = SettingsManager.AntiAliasingEnabled;
        antiAliasingCheckbox.onChange += AntiAliasingChanged;

        SetupResolution();
        prevResolutionButton.onClick.AddListener(() => { ChangeResolution(-1); });
        nextResolutionButton.onClick.AddListener(() => { ChangeResolution(1); });

        closeButton.onClick.AddListener(() => { gameObject.SetActive(false); });
    }

    private void FullScreenChanged(bool isFullscreen) {
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    private static void VSyncChanged(bool vSyncOn) {
        SettingsManager.VSyncEnabled = vSyncOn;
        ApplyGraphicsSettings();
    }

    private static void AntiAliasingChanged(bool antiAliasingOn) {
        SettingsManager.AntiAliasingEnabled = antiAliasingOn;
        ApplyGraphicsSettings();
    }

    public static void ApplyGraphicsSettings() {
        QualitySettings.vSyncCount = SettingsManager.VSyncEnabled ? 1 : 0;
        Application.targetFrameRate = SettingsManager.VSyncEnabled ? -1 : 240;
        QualitySettings.antiAliasing = SettingsManager.AntiAliasingEnabled ? 4 : 0;
    }

    private int resolutionIndex;
    private void ChangeResolution(int change) {
        resolutionIndex += change;
        if(resolutionIndex >= uniqResolutions.Length) {
            resolutionIndex = 0;
        } else if(resolutionIndex < 0) {
            resolutionIndex = uniqResolutions.Length - 1;
        }
        Resolution newResolution = uniqResolutions[resolutionIndex];
        SetResolution(newResolution.width, newResolution.height);
    }

    private void SetResolution(int x, int y) {
        Screen.SetResolution(x, y, Screen.fullScreenMode);
        resolutionText.text = x + " x " + y;
    }

    private void SetupResolution() {
        EnsureResolutions();
        Resolution currentRes = Screen.currentResolution;
        resolutionText.text = currentRes.width + " x " + currentRes.height;

        bool foundResolution = false;
        for (int i = 0; i < uniqResolutions.Length; i++) {
            Resolution resolution = uniqResolutions[i];
            if(resolution.width == currentRes.width && resolution.height == currentRes.height) {
                resolutionIndex = i;
                foundResolution = true;
                break;
            }
        }
        if (!foundResolution) {
            resolutionIndex = uniqResolutions.Length - 1;
        }
    }


    private Resolution[] uniqResolutions;
    private void EnsureResolutions() {
        if(uniqResolutions == null) {
            uniqResolutions = GetUniqueResolutions();
        }
    }

    private static Resolution[] GetUniqueResolutions() {
        Resolution[] resolutions = Screen.resolutions;
        Dictionary<ValueTuple<int, int>, Resolution> maxRefreshRates = new Dictionary<ValueTuple<int, int>, Resolution>();
        for (int i = 0; i < resolutions.GetLength(0); i++) {
            Resolution r = resolutions[i];
            ValueTuple<int, int> widthHeight = new ValueTuple<int, int>(r.width, r.height);
            float rateRatio = r.refreshRateRatio.numerator / (float)r.refreshRateRatio.denominator;
            if (maxRefreshRates.TryGetValue(widthHeight, out Resolution curretResolution)) {
                float currentRatio = curretResolution.refreshRateRatio.numerator / (float)curretResolution.refreshRateRatio.denominator;
                if (rateRatio > currentRatio) {
                    maxRefreshRates[widthHeight] = r;
                }
            } else {
                maxRefreshRates[widthHeight] = r;
            }
        }
        Resolution[] result = maxRefreshRates.Values.ToArray();
        Array.Sort(result, (Resolution a, Resolution b) => { return a.height.CompareTo(b.height); });
        return result;
    }
}
