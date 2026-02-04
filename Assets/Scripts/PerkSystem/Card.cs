using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class Card : MonoBehaviour, IPointerEnterHandler
{
    private PerkScriptableObject perk;
    private PerkController perkController;

    public string perk_id;

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
        perkController.PerformAction(perk);
        EventManager.Game.OnPerkPicked.Invoke();
    }

        public void OnPointerEnter(PointerEventData eventData)
    {
        EventManager.Game.OnPerkHover.Invoke(perk.description);
    }
}
