using UnityEngine;

public class AudioManager : MonoBehaviour {
    private const string AUDIO_MANAGER_PATH = "AudioManager";
    private static AudioManager instance;
    public static AudioManager Instance {
        get {
            if (instance == null) {
                GameObject audioManagerObject = (GameObject)Resources.Load(AUDIO_MANAGER_PATH);
                GameObject instantiated = Instantiate(audioManagerObject);
                DontDestroyOnLoad(instantiated);
                instance = instantiated.GetComponent<AudioManager>();
            }
            return instance;
        }
    }

    [Header("Sound Effects")]
    public AudioSource[] audioSources;
    private int audioSourceIndex = 0;

    public AudioClip incorrectRoomSound;
    public void PlayIncorrectRoomSound() {
        PlaySFX(incorrectRoomSound, 0.35f, 1);
    }

    public void PlaySFX(AudioClip clip, float volume, float pitch = 1) {
        AudioSource source = GetNextAudioSource();
        source.volume = volume * SettingsManager.SFXVolume;
        source.pitch = pitch;
        source.PlayOneShot(clip);
    }

    private AudioSource GetNextAudioSource() {
        AudioSource result = audioSources[audioSourceIndex];
        for (int i = 0; i < audioSources.Length; i++) {
            if (!result.isPlaying) {
                return result;
            }
            audioSourceIndex = (audioSourceIndex + 1) % audioSources.Length;
            result = audioSources[audioSourceIndex];
        }
        return result;
    }
}
