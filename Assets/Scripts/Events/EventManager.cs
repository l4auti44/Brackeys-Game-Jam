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
        public UnityAction OnWin;
        public UnityAction OnPerkPicked;
        public UnityAction<string> OnPerkHover;
        public UnityAction OnGameStopped;
        public UnityAction<Component> OnDie;
        public UnityAction<DialogSystem.DialogEvents> OnDialog;
        public UnityAction<DialogSystem.DialogEvents> OnTaskDialogCompleted;
        public UnityAction<float> OnTaskWaiting;
        public UnityAction OnTaskDialogFailed;
        public UnityAction<int> OnRadarChange;
        public UnityAction<int> OnEngineChange;
        public UnityAction<SystemBlueprint> OnBrokenSystem;
        
    }
}
