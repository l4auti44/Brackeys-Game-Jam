using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterVolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudioSource;
    private void Start()
    {
        GetComponent<UnityEngine.UI.Slider>().value = SoundManager.MasterVolume;
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = SoundManager.MasterVolume;
        }
    }

    public void OnValueChanged(float value)
    {
        SoundManager.MasterVolume = value;
    }

    public void ChangeMusicVolume()
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = SoundManager.MasterVolume;
        }
    }
}
