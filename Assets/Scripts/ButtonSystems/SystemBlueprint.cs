using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

abstract public class SystemBlueprint : MonoBehaviour
{
    public enum Modules
    {
        MissileModule,
        ShieldModule,
        ArrowModule,
        EngineModule,
        radarModule
    }

    [HideInInspector] public Modules module;
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
            currentColor.a = imageComponent.color.a / 2;

            imageComponent.color = currentColor;
            available = false;
        }
        else
        {
            currentColor.a = imageComponent.color.a * 2;
            imageComponent.color = currentColor;
            available = true;
        }
    }

    abstract public void DoAction();

    public bool CanDoAction()
    {
        if (broken)
        {
            CheckIfCanRepair();
            return false;
        }
        if (!available)
        {
            return false;
        }
        return true;
    }

    private void CheckIfCanRepair()
    {
        if (repairSys.canRepair)
        {
            repairSys.Repair();
        }
    }
}
