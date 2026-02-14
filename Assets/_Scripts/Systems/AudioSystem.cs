using System.Collections;
using UnityEngine;

/// <summary>
/// Insanely basic audio system which supports 3D sound.
/// Ensure you change the 'Sounds' audio source to use 3D spatial blend if you intend to use 3D sounds.
/// </summary>
public class AudioSystem : StaticInstance<AudioSystem> {

    public void PlayRdmPitchVol(AudioSource audioSource)
    {
        float pitch = audioSource.pitch;
        float volume = audioSource.volume;
        audioSource.pitch += Random.Range(-0.05f, 0.05f);
        audioSource.volume += Random.Range(-0.05f, 0.05f);
        audioSource.Play();
        StartCoroutine(WaitForAudio(audioSource, volume, pitch));
    }

    IEnumerator WaitForAudio(AudioSource audioSource, float oldVolume, float oldPitch)
    {
        yield return new WaitWhile(() => audioSource.isPlaying);

        audioSource.volume = oldVolume;
        audioSource.pitch = oldPitch;
    }
}