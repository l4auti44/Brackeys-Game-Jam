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
    [HideInInspector] public FlashButtonBroken flashButton;

    private Sprite pressedSprite;
    private Button button;

    [HideInInspector] public SubSystemsController subSystemsController;



    virtual public void Start()
    {
        flashButton = gameObject.GetComponent<FlashButtonBroken>();
        repairSys = GameObject.Find("GameManager").GetComponentInChildren<DestroyAndRepearSys>();
        button = GetComponent<Button>();
        pressedSprite = button.spriteState.pressedSprite;
        subSystemsController = transform.parent.GetComponent<SubSystemsController>();
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
        Color myColor;
        SpriteState state = button.spriteState;
        if (available)
        {
            myColor = Color.white;
            state.pressedSprite = pressedSprite;
        }
        else
        {
            UnityEngine.ColorUtility.TryParseHtmlString("#008851", out myColor);
            state.pressedSprite = null;
        }
        gameObject.GetComponent<Image>().color = myColor;
        button.spriteState = state;
    }
}
