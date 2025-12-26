using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class FlashButtonBroken : MonoBehaviour
{
    [SerializeField] private Sprite whiteImage;
    private Button button;
    private Sprite originalSprite;
    private Color originalColor;
   
    private void Awake()
    {
        button = GetComponent<Button>();
        originalSprite = button.image.sprite;
        originalColor = button.image.color;
    }
    public void StartFlash()
    {
        StartCoroutine(FlashCoroutine());
    }

    public void StopFlash()
    {
        StopAllCoroutines();
        button.image.sprite = originalSprite;
        button.image.color = originalColor;
    }

    private IEnumerator FlashCoroutine()
    {
        button.image.sprite = whiteImage;
        float flashDuration = 0.5f;
        float elapsed = 0f;

        // Ping-pong t between 0 and 1 so the color goes black -> red -> black, etc.
        while (true)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.PingPong(elapsed, flashDuration) / flashDuration; // 0..1..0 over 2*flashDuration
            button.image.color = Color.Lerp(Color.black, Color.red, t);
            yield return null;
        }
    }
}
