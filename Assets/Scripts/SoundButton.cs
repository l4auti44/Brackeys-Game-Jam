using UnityEngine;
using UnityEngine.UI;

public class SoundButton : Button
{
    protected override void OnEnable()
    {
        base.OnEnable();
        AddButtonSound(); // Automatically adds the click sound on enable
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        onClick.RemoveListener(PlayClickSound); // Ensure listener is removed when disabled
    }

    private void AddButtonSound()
    {
        onClick.AddListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
        SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
    }
}
