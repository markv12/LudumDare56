using UnityEngine;

public class FPSSounds : MonoBehaviour {
    [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

    public void PlayFootStepAudio() {
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        AudioClip stepClip = m_FootstepSounds[n];
        AudioManager.Instance.PlaySFX(stepClip, 1);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = stepClip;
    }
    public void PlayJumpSound() {
        AudioManager.Instance.PlaySFX(m_JumpSound, 1);
    }

    public void PlayLandingSound() {
        AudioManager.Instance.PlaySFX(m_LandSound, 1);
    }
}
