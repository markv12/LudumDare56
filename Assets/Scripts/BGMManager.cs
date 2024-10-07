using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour {
    public AudioSource audioSource;
    void Start() {
        audioSource.volume = SettingsManager.MusicVolume;
        SettingsManager.musicVolumeChanged += SettingsManager_musicVolumeChanged;
    }

    private void SettingsManager_musicVolumeChanged() {
        audioSource.volume = SettingsManager.MusicVolume;
    }
}
