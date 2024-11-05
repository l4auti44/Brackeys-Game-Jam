using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SoundManager
{

    /*
     *  Audio System that works by calling this functions from anywhere: SoundManager.PlaySound(Sound sound) for a 2d sound
     *  or SoundManager.PlaySound(Sound sound, Vector3 position) for a "3d" sound.
     */

    //All projects sounds
    public enum Sound
    {
        EnergyAtZeroWarning,
        StormImminent,
        RepairDone,
        HitByAsteroid,
        GameOver,
        DestinationReached,
        SpeakingCalm,
        SpeakingAngry,
        ButtonHover,
        ButtonClick,
        Hello,
        Destruct,
        Collect1,
        Collect2,
        Music1,
        Music2
    }

    private static GameObject OneShotGameObject, musicGameObject;
    private static AudioSource oneShotAudioSource, musicAudioSource;

    private static float audioClipVolume;



    //Timers for sounds that need a delay between each PlaySound()
    public static Dictionary<Sound, float> soundTimerDictionary;

    //NEED to be called in an Awake function (like in the gameManager.cs)
    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        //INITIALIZE HERE EACH SOUND THAT NEED A TIMER
        //EXAMPLE: soundTimerDictionary[Sound.PlayerMove] = 0f;
    }

    
    public static void PlayBackgroundMusic(Sound sound)
    {
        if (CanPlaySound(sound))
        {
            if (musicGameObject == null)
            {
                musicGameObject = new GameObject("Music Source");
                musicAudioSource = musicGameObject.AddComponent<AudioSource>();
                musicAudioSource.loop = true;
            }
            GetAudioClip(sound);
            float targetVolume = audioClipVolume;
            // If the music is already playing, start the fade-out before changing the track.
            if (musicAudioSource.isPlaying)
            {
                // Start coroutine for fade-out, then change music, then fade-in
                musicAudioSource.gameObject.AddComponent<MonoBehaviourHelper>().StartCoroutine(
                    FadeOutAndChangeTrack(sound, targetVolume, 0.5f)
                );
            }
            else
            {
                // Start playing immediately if no music is currently playing
                StartMusic(sound, targetVolume);
            }
        }
    }
    #region BackgroundMusic helpers
    private static IEnumerator FadeOutAndChangeTrack(Sound sound, float targetVolume, float fadeDuration)
    {
        // Fade out
        yield return FadeVolume(0f, fadeDuration);

        // Change the track
        AudioClip audioClip = GetAudioClip(sound);
        musicAudioSource.clip = audioClip;
        musicAudioSource.Play();

        // Fade in
        yield return FadeVolume(targetVolume, fadeDuration);
    }

    private static IEnumerator FadeVolume(float targetVolume, float duration)
    {
        float startVolume = musicAudioSource.volume;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            musicAudioSource.volume = Mathf.Lerp(startVolume, targetVolume, (Time.time - startTime) / duration);
            yield return null;
        }

        musicAudioSource.volume = targetVolume;
    }

    private static void StartMusic(Sound sound, float targetVolume)
    {
        AudioClip audioClip = GetAudioClip(sound);
        musicAudioSource.clip = audioClip;
        musicAudioSource.volume = targetVolume;
        musicAudioSource.Play();
    }

    // Helper MonoBehaviour class to start coroutines in a static context
    public class MonoBehaviourHelper : MonoBehaviour { }

    #endregion
    public static void PlaySound(Sound sound, Vector3 position)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);
            // Could have some audio options in here like: audioSource.maxDistance = 100f;
            audioSource.Play();

            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    }

    public static void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound))
        {
            if (OneShotGameObject == null)
            {
                OneShotGameObject = new GameObject("One Shot Sound");
                oneShotAudioSource = OneShotGameObject.AddComponent<AudioSource>();
            }
            AudioClip audioClip = GetAudioClip(sound);
            oneShotAudioSource.volume = audioClipVolume;
            oneShotAudioSource.PlayOneShot(audioClip);
        }
    }

    //Random choose a sound from a given list
    public static void PlaySound(Sound[] sounds)
    {
        Sound sound = sounds[Random.Range(0, sounds.Length)];
        if (CanPlaySound(sound))
        {
            if (OneShotGameObject == null)
            {
                OneShotGameObject = new GameObject("One Shot Sound");
                oneShotAudioSource = OneShotGameObject.AddComponent<AudioSource>();
            }
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
    }


    //This is an EXTENSION from Button class.
    //NEED to be called from every button that needs this behaviour. EX: GameObject.Find("ButtonUPtest").GetComponent<Button>().AddButtonSounds();
    public static void AddButtonSounds(this Button button)
    {
        button.onClick.AddListener(() => SoundManager.PlaySound(Sound.ButtonClick));
 
    }


    //Add here Sounds that need timer between each PlaySound()
    ///<example>
    ///<code>
    /// <![CDATA[
    ///          case Sound.PlayerMove:
    ///          if (soundTimerDictionary.ContainsKey(sound)){
    ///             float lastTimePlayed = soundTimerDictionary[sound];
    ///             float playerMoveTimerMax = .05f;  THIS IS THE DELAY THAT WE WANT
    ///             if (lastTimePlayer + playerMoveTimerMax < Time.time) {
    ///                   soundTimerDictionary[sound] = Time.time;
    ///                   return true;
    ///             } else {
    ///                   return false;
    ///             }
    ///          }
    /// ]]>
    ///</code>
    ///</example>
    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default: return true;

            
        }
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach(GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                audioClipVolume = soundAudioClip.volume;
                return soundAudioClip.audioClip;
            }
        }

        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

}
