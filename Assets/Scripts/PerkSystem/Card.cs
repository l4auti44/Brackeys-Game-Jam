using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private PerkScriptableObject perk;
    private PerkController perkController;

    void Start()
    {
        perkController = GameObject.Find("Canvas Perks").GetComponent<PerkController>();
        perk = GameAssets.i.perksPool[Random.Range(0, GameAssets.i.perksPool.Length)];
        PopulatePerkData(perk);
    }
    public void PopulatePerkData(PerkScriptableObject perk)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = perk.title;
        //description.text = perk.description;
        //sprite.sprite = perk.sprite;
    }

    public void PerformeAction()
    {
        perkController.PerformAction(perk.action);
    }

}
