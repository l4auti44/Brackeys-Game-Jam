using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load("GameAssets") as GameObject).GetComponent<GameAssets>();
            return _i;
        }
        
    }

    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
        [Range(0, 1)]public float volume = 1;
    }


    public DialogueAudioPool[] dialogueAudioPools;
    [System.Serializable]
    public class DialogueAudioPool
    {
        public DialogSystem.DialogPool dialogPool;
        public AudioClip[] audios;
        [Range(0, 1)] public float volume = 1;
    }
}
