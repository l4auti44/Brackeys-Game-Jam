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
       
    }


    public void PopulatePerkData(PerkScriptableObject _perk)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = _perk.title;
        perk = _perk;
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
