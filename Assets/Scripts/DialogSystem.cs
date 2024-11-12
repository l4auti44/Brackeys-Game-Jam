using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    public float timeForEachCharacter = 0.2f;
    public enum DialogEvents
    {
        NoGetHit,
        Event2,
    }

    public enum DialogPool
    {
        Calm,
        Angry
    }
    private TMPro.TMP_Text textComponent;

    public DialogEventsList[] dialogEventsList;

    [System.Serializable]
    public class DialogEventsList
    {
        public DialogEvents dialogEvent;
        public DialogPool dialogPool;
        [TextArea]
        public string taskText, failText, goodText;
        public float timeForTask = 30f;
        private bool isDone = false;
    }

    private DialogEventsList currentDialog;

    private bool isWritting = false;

    [HideInInspector]
    public DialogEvents dialogTEST;

    private void Start()
    {
        textComponent = GameObject.Find("DialogText").GetComponent<TMPro.TMP_Text>();
    }

    private void Update()
    {
        if (currentDialog != null)
        {
            switch (currentDialog.dialogEvent)
            {
                case DialogEvents.NoGetHit:
                    break;
            }
        }
    }

    private void OnEnable()
    {
        EventManager.Game.OnDialog += StartDialog;
        EventManager.Player.OnImpact += CheckOnImpactTask;
    }

    private void OnDisable()
    {
        EventManager.Game.OnDialog -= StartDialog;
        EventManager.Player.OnImpact -= CheckOnImpactTask;
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
            StartCoroutine(TypeText(currentDialog.taskText));
        }
        StartCoroutine(WaitingForAction());
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
        isWritting = false;
        SoundManager.StopDialogueSound();
    }
    private void TaskCompleted()
    {
        StopWritting();
        StartCoroutine(TypeText(currentDialog.goodText));
        StartCoroutine(WaitToDeleteText());
        currentDialog = null;
    }

    public void TaskFailed()
    {
        StopWritting();
        StartCoroutine(TypeText(currentDialog.failText));
        StartCoroutine(WaitToDeleteText());
        currentDialog = null;
    }


    private void CheckOnImpactTask(Component comp)
    {
        if (currentDialog != null)
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

        switch (currentDialog.dialogEvent)
        {
            case DialogEvents.NoGetHit:
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
    private IEnumerator TypeText(string message)
    {
        SoundManager.PlayDialogueSound(currentDialog.dialogPool);
        isWritting = true;
        textComponent.text = "";
        foreach (char letter in message)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(timeForEachCharacter);
        }
        isWritting = false;
        SoundManager.StopDialogueSound();
    }
}
