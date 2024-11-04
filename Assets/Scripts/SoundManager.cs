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
        Alarm1,
        Alarm2,
        Repair,
        Speaking,
        Death,
        Impact,
        ButtonOver,
        ButtonClick
    }


    //Timers for sounds that need a delay between each PlaySound()
    public static Dictionary<Sound, float> soundTimerDictionary;

    private static GameObject OneShotGameObject;
    private static AudioSource oneShotAudioSource;


    //NEED to be called in an Awake function (like in the gameManager.cs)
    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        //INITIALIZE HERE EACH SOUND THAT NEED A TIMER
        //EXAMPLE: soundTimerDictionary[Sound.PlayerMove] = 0f;
    }


    
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
    /*EXAMPLE:
             * case Sound.PlayerMove:
             * if (soundTimerDictionary.ContainsKey(sound)){
             *    float lastTimePlayed = soundTimerDictionary[sound];
             *    float playerMoveTimerMax = .05f;  THIS IS THE DELAY THAT WE WANT
             *    if (lastTimePlayer + playerMoveTimerMax < Time.time) {
             *          soundTimerDictionary[sound] = Time.time;
             *          return true;
             *    } else {
             *          return false;
             *    }
             * }
             */
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
                return soundAudioClip.audioClip;
            }
        }

        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }
}
