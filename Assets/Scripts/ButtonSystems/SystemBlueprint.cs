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



    virtual public void Start()
    {
        repairSys = GameObject.Find("GameManager").GetComponentInChildren<DestroyAndRepearSys>();
    }

    public void SwitchAvailable()
    {
        if (available)
        {

            available = false;
        }
        else
        {

            available = true;
        }
        ChangeApareance();
    }

    public void SwitchAvailable(bool state)
    {
        if (state == false)
        {

            available = false;
        }
        else
        {

            available = true;
        }
        ChangeApareance();
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

    public void ChangeApareance()
    {
        if (available)
        {
            gameObject.GetComponent<SoundButton>().interactable = true;
        }
        else
        {
            gameObject.GetComponent<SoundButton>().interactable = false;

        }
    }
}
