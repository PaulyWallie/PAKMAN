using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [System.Serializable]
    public struct SoundEffect
    {
        public SoundType type;
        public AudioClip clip; // This can also be an AudioRandomContainer asset
        public AudioMixerGroup mixerGroup;
    }

    [Header("Configuration")]
    public AudioSource sfxSource;
    public AudioSource bgmSource;

    [Header("Sound Lists")]
    public List<SoundEffect> sfxList;
    public List<SoundEffect> musicList;

    private Dictionary<SoundType, SoundEffect> sfxDictionary = new Dictionary<SoundType, SoundEffect>();
    private Dictionary<SoundType, SoundEffect> musicDictionary = new Dictionary<SoundType, SoundEffect>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (var sfx in sfxList)
        {
            if (!sfxDictionary.ContainsKey(sfx.type))
                sfxDictionary.Add(sfx.type, sfx);
        }

        foreach (var music in musicList)
        {
            if (!musicDictionary.ContainsKey(music.type))
                musicDictionary.Add(music.type, music);
        }
    }

    public void PlaySFX(SoundType type)
    {
        if (sfxDictionary.TryGetValue(type, out SoundEffect sfx))
        {
            // Route to the specific group for this sound (Player vs Enemy vs UI etc.)
            sfxSource.outputAudioMixerGroup = sfx.mixerGroup;
            
            // PlayOneShot handles overlapping sounds. 
            // Note: If using AudioRandomContainer, assign it to sfxSource.clip or use Play().
            sfxSource.PlayOneShot(sfx.clip);
        }
        else
        {
            Debug.LogWarning($"SFX of type {type} not found in AudioManager!");
        }
    }

    public void PlayMusic(SoundType type)
    {
        if (musicDictionary.TryGetValue(type, out SoundEffect music))
        {
            bgmSource.Stop();
            bgmSource.clip = music.clip;
            bgmSource.outputAudioMixerGroup = music.mixerGroup;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void EndLevelVictoryMusic() => PlayMusic(SoundType.LevelEnd);
    public void PlayBossMusic() => PlayMusic(SoundType.BossMusic);
    public void StopBossMusic() => PlayMusic(SoundType.BGM);
}
