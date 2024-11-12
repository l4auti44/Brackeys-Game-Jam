using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static readonly PlayerEvents Player = new PlayerEvents();
    public static readonly GameEvents Game = new GameEvents();
    public class PlayerEvents
    {
        public UnityAction<Component> OnImpact;
    }

    public class GameEvents
    {
        public UnityAction<Component> OnWin;
        public UnityAction<Component> OnDie;
        public UnityAction<DialogSystem.DialogEvents> OnDialog;
        public UnityAction<DialogSystem.DialogEvents> OnTaskDialogCompleted;
        public UnityAction OnTaskDialogFailed;
        
    }
    
}
