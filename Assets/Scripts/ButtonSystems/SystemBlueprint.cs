using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

abstract public class SystemBlueprint : MonoBehaviour
{
    public enum SubSys
    {
        MissileModule,
        ShieldModule,
        ArrowModule,
        repairModule
    }

    [HideInInspector] public SubSys module;
    private bool available = true;
    [HideInInspector] public bool broken = false;
    [HideInInspector] public DestroyAndRepearSys repairSys;
    private Image imageComponent;
    private Color currentColor;

    private void Awake()
    {
        imageComponent = GetComponent<Image>();
        currentColor = imageComponent.color;
    }
    virtual public void Start()
    {
        repairSys = GameObject.Find("GameManager").GetComponentInChildren<DestroyAndRepearSys>();
    }

    public void SwitchAvailable()
    {
        if (available)
        {
            currentColor = imageComponent.color;
            currentColor.a = 0.5f;

            imageComponent.color = currentColor;
            available = false;
        }
        else
        {
            currentColor.a = 1;
            imageComponent.color = currentColor;
            available = true;
        }
    }

    public void SwitchAvailable(bool state)
    {
        if (state == false)
        {
            currentColor = imageComponent.color;
            currentColor.a = 0.5f;

            imageComponent.color = currentColor;
            available = false;
        }
        else
        {
            currentColor.a = 1;
            imageComponent.color = currentColor;
            available = true;
        }
    }

    abstract public void DoAction();

    public bool CanDoAction()
    {
        if (broken)
        {
            return false;
        }
        if (!available)
        {
            return false;
        }
        return true;
    }

    public bool GetAvailable()
    {
        return available;
    }
}
