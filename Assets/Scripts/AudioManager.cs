using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager current;

    [Header("Music")]
    public AudioClip levelMusic;
    public AudioClip MenuMusic;
    public AudioClip gameoverMusic;
    public AudioClip victoryMusic;
    [Header("Player")]
    public AudioClip jump;
    public AudioClip coinPickup;
    public AudioClip hitHurt;
    public AudioClip heartPickup;
    public AudioClip extaLifePickup;
    public AudioClip skullPickup;
    [Header("Mixer")]
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup playerGroup;

    AudioSource music, player;

    void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
            return;
        }
        current = this;

        music = gameObject.AddComponent<AudioSource>();
        player = gameObject.AddComponent<AudioSource>();

        music.outputAudioMixerGroup = musicGroup;
        player.outputAudioMixerGroup = playerGroup;
        PlayLevelMusic();
    }

   public void PlayLevelMusic()
    {
        current.music.clip = current.levelMusic;
        current.music.loop = true;
        current.music.Play();
    }

    public void PlayMenuMusic()
    {
        if (current == null)
            return;

        current.music.Stop();
        current.music.clip = current.MenuMusic;
        current.music.Play();
    }

    public void PlayGameoverMusic()
    {
        if (current == null)
            return;

        current.music.Stop();
        current.music.clip = current.gameoverMusic;
        current.music.Play();
    }

    public void PlayVictoryMusic()
    {
        if (current == null)
            return;

        current.music.Stop();
        current.music.clip = current.victoryMusic;
        current.music.Play();
    }

    public void PlayJumpAudio()
    {
        if (current == null)
            return;

        current.player.clip = current.jump;
        current.player.Play();
    }


    public void PlayextaLifeAudio()
    {
        if (current == null)
            return;

        current.player.clip = current.extaLifePickup;
        current.player.Play();
    }
    public void PlayCoinAudio()
    {
        if (current == null)
            return;

        current.player.clip = current.coinPickup;
        current.player.Play();
    }

    public void PlayHeartPickupAudio()
    {
        if (current == null)
            return;

        current.player.clip = current.heartPickup;
        current.player.Play();
    }

    public void PlaySkullPickupAudio()
    {
        if (current == null)
            return;

        current.player.clip = current.skullPickup;
        current.player.Play();
    }
    public void PlayHitHurtAudio()
    {
        if (current == null)
            return;

        current.player.clip = current.hitHurt;
        current.player.Play();
    }
}


