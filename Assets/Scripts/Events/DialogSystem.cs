using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    public float timeForEachCharacter = 0.2f;
    public Animator halAnimator;
    public enum DialogEvents
    {
        NoGetHit,
        KeepRadarAt,
        KeepEngineAt,
    }

    public enum DialogPool
    {
        Calm,
        Angry
    }
    private TMPro.TMP_Text textComponent;
    private GameManager gameManager;

    public DialogEventsList[] dialogEventsList;

    [System.Serializable]
    public class DialogEventsList
    {
        public DialogEvents dialogEvent;
        [TextArea]
        public string taskText, failText, goodText;
        public float timeForTask = 30f;
        private bool isDone = false;
    }

    private DialogEventsList currentDialog;

    private bool isWritting = false;

    private int CurrentLevelTask;

    [HideInInspector]
    public DialogEvents dialogTEST;

    private void Start()
    {
        textComponent = GameObject.Find("DialogText").GetComponent<TMPro.TMP_Text>();
        gameManager = transform.parent.GetComponent<GameManager>();
    }


    private void OnEnable()
    {
        EventManager.Game.OnDialog += StartDialog;
        EventManager.Player.OnImpact += CheckOnImpactTask;
        EventManager.Game.OnRadarChange += RadarLevelCheck;
        EventManager.Game.OnEngineChange += EngineLevelCheck;
    }

    private void OnDisable()
    {
        EventManager.Game.OnDialog -= StartDialog;
        EventManager.Player.OnImpact -= CheckOnImpactTask;
        EventManager.Game.OnRadarChange -= RadarLevelCheck;
        EventManager.Game.OnEngineChange -= EngineLevelCheck;
    }

    private void StartDialog(DialogEvents dialog)
    {
        if (isWritting)
        {
            StopWritting();
        }
        currentDialog = GetDialog(dialog);
        if (currentDialog != null)
        {
            StopAllCoroutines();
            StartCoroutine(TypeText(currentDialog.taskText, DialogPool.Calm));
        }
        
    }

    private DialogEventsList GetDialog(DialogEvents dialogEvent)
    {
        foreach(DialogEventsList dialogEventClass in dialogEventsList)
        {
            if (dialogEvent == dialogEventClass.dialogEvent)
            {
                return dialogEventClass;
            }
        }

        Debug.LogError("Dialog Event not found!");
        return null;
    }
    private void StopWritting()
    {
        StopAllCoroutines();
        
        textComponent.text = "";
        halAnimator.SetBool("Talking", false);
        isWritting = false;
        SoundManager.StopDialogueSound();
    }
    private void TaskCompleted()
    {
        StopWritting();
        StartCoroutine(TypeText(currentDialog.goodText, DialogPool.Calm));
        StartCoroutine(WaitToDeleteText());
        halAnimator.SetBool("Completed", true);
        currentDialog = null;
    }

    public void TaskFailed()
    {
        StopWritting();
        StartCoroutine(TypeText(currentDialog.failText, DialogPool.Angry));
        StartCoroutine(WaitToDeleteText());
        halAnimator.SetBool("Failed", true);
        currentDialog = null;
        EventManager.Game.OnTaskDialogFailed.Invoke();
    }

    private void RadarLevelCheck(int lv)
    {
        if (currentDialog != null && isWritting == false)
        {
            if (currentDialog.dialogEvent == DialogEvents.KeepRadarAt)
            {
                if (lv != 2)
                {
                    TaskFailed();
                }
            }
        }
    }

    private void EngineLevelCheck(int lv)
    {
        if (currentDialog != null && isWritting == false)
        {
            if (currentDialog.dialogEvent == DialogEvents.KeepEngineAt)
            {
                if (lv != 2)
                {
                    TaskFailed();
                }
            }
        }
    }
    private void CheckOnImpactTask(Component comp)
    {
        if (currentDialog != null && isWritting == false)
        {
            if (currentDialog.dialogEvent == DialogEvents.NoGetHit)
            {
                StopCoroutine("WaitingForAction");
                TaskFailed();
            }
        }
        
    }

    public void TestEvent()
    {
        StartDialog(dialogTEST);
    }
    private IEnumerator WaitingForAction()
    {
        float time = currentDialog.timeForTask;
        EventManager.Game.OnTaskWaiting.Invoke(1.57f / time);

        switch (currentDialog.dialogEvent)
        {
            case DialogEvents.NoGetHit:
                yield return new WaitForSeconds(time);
                TaskCompleted();
                break;
            case DialogEvents.KeepRadarAt:
                CurrentLevelTask = (int)gameManager.radarLV;
                if (CurrentLevelTask != 2)
                {
                    TaskFailed();
                    break;
                }
                yield return new WaitForSeconds(time);
                TaskCompleted();
                break;
            case DialogEvents.KeepEngineAt:
                CurrentLevelTask = (int)gameManager.positionLV;
                if (CurrentLevelTask != 2)
                {
                    TaskFailed();
                    break;
                }
                yield return new WaitForSeconds(time);
                TaskCompleted();
                break;

            default:
                yield return new WaitForSeconds(time);
                TaskFailed();
                break;
        }
        
    }
    private IEnumerator WaitToDeleteText()
    {
        yield return new WaitForSeconds(10f);
        StopWritting();
    }
    private IEnumerator TypeText(string message, DialogPool audioPool)
    {
        SoundManager.PlayDialogueSound(audioPool);
        halAnimator.SetBool("Talking", true);
        halAnimator.SetBool("Failed", false);
        halAnimator.SetBool("Completed", false);
        isWritting = true;
        textComponent.text = "";
        foreach (char letter in message)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(timeForEachCharacter);
        }

        SoundManager.StopDialogueSound();
        yield return new WaitForSeconds(2f);
        isWritting = false;
        halAnimator.SetBool("Talking", false);
        if (currentDialog != null)
        {
            StartCoroutine(WaitingForAction());
        }
    }
}
