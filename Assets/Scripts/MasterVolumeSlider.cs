using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterVolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudioSource;
    private float startVolume;
    private void Awake()
    {
        
        if (musicAudioSource != null)
        {
            startVolume = musicAudioSource.volume;
            musicAudioSource.volume = startVolume * SoundManager.MasterVolume;
        }
        GetComponent<UnityEngine.UI.Slider>().value = SoundManager.MasterVolume;
    }

    private void Start()
    {

    }

    public void OnValueChanged(float value)
    {
        SoundManager.MasterVolume = value;
    }

    public void ChangeMusicVolume()
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = startVolume * SoundManager.MasterVolume;
        }
    }
}
