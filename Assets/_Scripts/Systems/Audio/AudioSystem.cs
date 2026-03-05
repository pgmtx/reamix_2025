using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioSystem : StaticInstance<AudioSystem>
{
    public Sound[] Sounds2D;
    public Sound[] Sounds3D;
    public Sound[] Footsteps;

    protected override void Awake()
    {
        base.Awake();

        foreach (Sound s in Sounds2D)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {
        Play2DSound("white noise");
    }

    public void Play2DSound(string name)
    {
        Sound s = Array.Find(Sounds2D, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Son: " + name + " introuvable !!!");
            return;
        }
        s.source.Play();
    }

    public void Play2DSoundRdmPitchVol(string name)
    {
        Sound s = Array.Find(Sounds2D, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Son: " + name + " introuvable !!!");
            return;
        }

        float pitch = s.source.pitch;
        float volume = s.source.volume;
        s.source.pitch += UnityEngine.Random.Range(-0.05f, 0.05f);
        s.source.volume += UnityEngine.Random.Range(-0.05f, 0.05f);
        s.source.Play();
        StartCoroutine(WaitForAudio(s.source, volume, pitch));
    }

    public void PlayFootstep(Vector3 position)
    {
        Sound s = Footsteps[UnityEngine.Random.Range(0, Footsteps.Length)];

        // Copié de la fonction de unity  de base
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = s.clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = s.volume + UnityEngine.Random.Range(-0.05f, 0.05f);
        audioSource.pitch = s.pitch + UnityEngine.Random.Range(-0.05f, 0.05f);
        audioSource.Play();
        //UnityEngine.Object.Destroy(gameObject, s.clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    public void Play3DSound(string name, Vector3 position)
    {
        Sound s = Array.Find(Sounds3D, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Son: " + name + " introuvable !!!");
            return;
        }

        AudioSource.PlayClipAtPoint(s.clip, position);
    }

    public void Play3DSoundRdmPitchVol(string name, Vector3 position)
    {
        Sound s = Array.Find(Sounds3D, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Son: " + name + " introuvable !!!");
            return;
        }

        // Copié de la fonction de unity  de base
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = s.clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = s.volume + UnityEngine.Random.Range(-0.05f, 0.05f);
        audioSource.pitch = s.pitch + UnityEngine.Random.Range(-0.05f, 0.05f);
        audioSource.Play();
        UnityEngine.Object.Destroy(gameObject, s.clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    public void Play3DSoundRdmPitchVol(string name, Vector3 position, float randomVolume, float randomPitch)
    {
        Sound s = Array.Find(Sounds3D, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Son: " + name + " introuvable !!!");
            return;
        }

        // Copié de la fonction de unity  de base
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = s.clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = s.volume + randomVolume;
        audioSource.pitch = s.pitch + randomPitch;
        audioSource.Play();
        UnityEngine.Object.Destroy(gameObject, s.clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    public void Play3DSoundRdmPitchVol(Sound s, Vector3 position)
    {
        // Copié de la fonction de unity  de base
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = s.clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = s.volume + UnityEngine.Random.Range(-0.05f, 0.05f);
        audioSource.pitch = s.pitch + UnityEngine.Random.Range(-0.05f, 0.05f);
        audioSource.Play();
        UnityEngine.Object.Destroy(gameObject, s.clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }

    IEnumerator WaitForAudio(AudioSource audioSource, float oldVolume, float oldPitch)
    {
        yield return new WaitWhile(() => audioSource.isPlaying);

        audioSource.volume = oldVolume;
        audioSource.pitch = oldPitch;
    }
}