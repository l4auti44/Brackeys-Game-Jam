using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Perk", menuName = "ScriptableObjects/PerkScriptableObject", order = 1)]
public class PerkScriptableObject : ScriptableObject
{
    public string title;
    [TextArea(3,10)]
    public string description;

    public Sprite sprite;

    //public EventManager.PerkEvents action;
}
