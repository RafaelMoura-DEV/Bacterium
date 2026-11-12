using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : IPersistentSingleton<AudioManager>
{
    public enum SFXs
    {
        Defeat,
        Victory
    }

    public enum Soundtracks
    {
        Menu,
        Game,
    }

    [SerializeField] private AudioClip[] sfxClips;
    [SerializeField] private AudioClip[] soundtrackClips;
    [SerializeField] private AudioMixerGroup masterMix;
    [SerializeField] private AudioSource trackSource;

    // --------------------------------------------------------------------------------------- //

    private void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            CreateAudioSource();
        }
    }

    public void SwitchSoundtrack(Soundtracks enumAudioClip, float volume, bool play)
    {
        if (trackSource != null)
        {
            trackSource.clip = soundtrackClips[(int)enumAudioClip];
            trackSource.volume = volume;
            if (play) trackSource.Play();
            else trackSource.Pause();
        }
    }

    public void StopSoundtrack()
    {
        if (trackSource != null) trackSource.Stop();
    }

    public void PlaySound(SFXs enumAudioClip, float volume, float pitch)
    {
        StartCoroutine(PlayAudioCoroutine(enumAudioClip, volume, pitch));
    }

    public void SetVolume(float value)
    {
        // codigo de controlar audio
    }

    AudioSource GetAvailableAudioSource()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            AudioSource audioSource = transform.GetChild(i).GetComponent<AudioSource>();
            if (audioSource != null && audioSource.clip == null) return audioSource;
        }

        return null;
    }

    AudioSource CreateAudioSource()
    {
        GameObject audioPlayerObj = new GameObject();
        audioPlayerObj.transform.parent = transform;
        audioPlayerObj.name = "Audio Object";
        AudioSource audioSource = audioPlayerObj.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.outputAudioMixerGroup = masterMix;
        return audioSource;
    }

    private IEnumerator PlayAudioCoroutine(SFXs enumAudioClip, float volume, float pitch)
    {
        AudioSource audioSource = GetAvailableAudioSource();

        if (audioSource == null) audioSource = CreateAudioSource();

        audioSource.clip = sfxClips[(int)enumAudioClip];
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length + 2.0f);

        audioSource.Stop();
        audioSource.clip = null;
    }
}
