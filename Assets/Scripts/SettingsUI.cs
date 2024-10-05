using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour {
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    public const float MAX_LOOK_SPEED = 1.75f;
    private const float MIN_LOOK_SPEED = 0.1f;
    public Slider lookSpeedSlider;

    public Button closeButton;

    public Player player;

    public Button restartButton;
    public Button exitButton;

    public GameObject graphicsSettingsUI;
    public Button graphicsSettingsButton;

    public GameObject creditsPanel;
    public Button creditsButton;
    public Button closeCreditsButton;

    private void Awake() {
        musicVolumeSlider.value = SettingsManager.MusicVolume;
        sfxVolumeSlider.value = SettingsManager.SFXVolume;
        lookSpeedSlider.value = SettingsManager.LookSpeed / MAX_LOOK_SPEED;
        musicVolumeSlider.onValueChanged.AddListener(MusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(SfxVolumeChanged);
        lookSpeedSlider.onValueChanged.AddListener(LookSpeedChanged);

        closeButton.onClick.AddListener(Close);
        restartButton.onClick.AddListener(Restart);
        exitButton.onClick.AddListener(Exit);

        creditsButton.onClick.AddListener(() => { creditsPanel.SetActive(true); });
        closeCreditsButton.onClick.AddListener(() => { creditsPanel.SetActive(false); });

        graphicsSettingsButton.onClick.AddListener(() => { graphicsSettingsUI.SetActive(true); });
    }

    private void Update() {
        if (InputUtil.Cancel.WasPressed) {
            Close();
        }
    }

    private Action onExit;
    public void Open(bool drawingMode, Action _onExit) {
        gameObject.SetActive(true);
        onExit = _onExit;

        restartButton.gameObject.SetActive(!drawingMode);
#if UNITY_WEBGL
        exitButton.gameObject.SetActive(false);
        graphicsSettingsButton.gameObject.SetActive(false);
#else
        exitButton.gameObject.SetActive(!drawingMode);
        graphicsSettingsButton.gameObject.SetActive(true);
#endif
    }

    private void Close() {
        gameObject.SetActive(false);
        graphicsSettingsUI.SetActive(false);
        creditsPanel.SetActive(false);
        onExit?.Invoke();
    }

    public static void Exit() {
#if UNITY_EDITOR
        if (Application.isEditor)
            UnityEditor.EditorApplication.isPlaying = false;
        else
#endif
            Application.Quit();
    }

    private void Restart() {
        gameObject.SetActive(false);
        graphicsSettingsUI.SetActive(false);
        creditsPanel.SetActive(false);
        LoadingScreen.LoadScene("MainScene");
    }

    private void MusicVolumeChanged(float volume) {
        SettingsManager.MusicVolume = volume;
    }

    private void SfxVolumeChanged(float volume) {
        SettingsManager.SFXVolume = volume;
    }

    private void LookSpeedChanged(float speed) {
        SettingsManager.LookSpeed = Mathf.Max(MIN_LOOK_SPEED, speed * MAX_LOOK_SPEED);
    }
}
