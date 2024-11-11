using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    public float timeForEachCharacter = 0.2f;
    public enum DialogEvents
    {
        Event1,
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
        [TextArea]
        public string text;
        public DialogPool dialogPool;
        private bool isDone = false;
    }

    private bool isWritting = false;
    public DialogEvents dialogTEST;

    private void Start()
    {
        textComponent = GetComponent<TMPro.TMP_Text>();
    }

    private void OnEnable()
    {
        EventManager.Game.OnDialog += StartDialog;
        EventManager.Game.OnTaskDialogCompleted += TaskDialogCompleted;
    }

    private void OnDisable()
    {
        EventManager.Game.OnDialog -= StartDialog;
        EventManager.Game.OnTaskDialogCompleted -= TaskDialogCompleted;
    }

    private void StartDialog(DialogEvents dialog)
    {
        if (isWritting)
        {
            StopAllCoroutines();
            isWritting = false;
            SoundManager.StopDialogueSound();
        }
        var dialogEventClass = GetDialog(dialog);
        if (dialogEventClass != null)
        {
            StartCoroutine(TypeText(dialogEventClass.text));
        }
        SoundManager.PlayDialogueSound(dialogEventClass.dialogPool);
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

    private void TaskDialogCompleted(Component comp)
    {
        if (textComponent.text.Length != 0)
        {
            StopAllCoroutines();
            textComponent.text = "";
        }
    }

    public void TestEvent()
    {
        StartDialog(dialogTEST);
    }

    private IEnumerator TypeText(string message)
    {
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
