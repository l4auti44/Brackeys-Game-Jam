using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private TextMeshProUGUI title, description;
    private Image sprite;

    // Start is called before the first frame update
    void Start()
    {
        title = transform.Find("Title").GetComponent <TextMeshProUGUI>();
        description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
        sprite = transform.Find("Sprite Image").GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PopulatePerkData(GameAssets.i.perksPool[Random.Range(0, GameAssets.i.perksPool.Length)]);
        }
    }

    public void PopulatePerkData(PerkScriptableObject perk)
    {
        title.text = perk.title;
        description.text = perk.description;
        sprite.sprite = perk.sprite;
    }

}
