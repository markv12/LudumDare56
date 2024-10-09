using UnityEngine;

public class PlaySoundOnTrigger : MonoBehaviour {
    public AudioSource audioSource;
    private bool triggered = false;
    private void OnTriggerEnter(Collider other) {
        if (!triggered && other.gameObject.IsPlayer()) {
            triggered = true;
            audioSource.volume = SettingsManager.SFXVolume;
            audioSource.Play();
        }
    }
}
