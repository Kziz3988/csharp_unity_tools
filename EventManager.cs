using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventPermission {
    Transparent = 0, //Not accessible via Lua
    ListenOnly = 1, //Only allows AddListener
    ListenTrigger = 2, //Allows AddListener and TriggerEvent
    FullAccess = 3 //Allows AddListener, TriggerEvent and RemoveListener
}

public enum EventName {
    AnimatorStateEntered,
    AnimatorStateExited,
    ButtonClicked,
    EnemyHurt,
    GameStarted,
    LanguageSwitched,
    ModLoaded,
    PlayerHurt,
    ShowDamage,
    ShowHealth,
    ShowInventory,
    SwitchScene,
    TaskComplete,
    Tick,
}

public class EventManager : MonoBehaviour {
	public static EventManager instance; //Singleton pattern
    readonly Dictionary<string, Delegate> events = new Dictionary<string, Delegate>();
    Dictionary<EventName, EventPermission> eventPermission; 

	void Awake() {
		if(instance == null) instance = this;
        else Destroy(gameObject);
		Init();
	}

	void Init() {
		events.Clear();
        eventPermission = new Dictionary<EventName, EventPermission>{
            {EventName.AnimatorStateEntered, EventPermission.Transparent},
            {EventName.AnimatorStateExited, EventPermission.Transparent},
            {EventName.ButtonClicked, EventPermission.Transparent},
            {EventName.EnemyHurt, EventPermission.ListenOnly},
            {EventName.GameStarted, EventPermission.Transparent},
            {EventName.LanguageSwitched, EventPermission.Transparent},
            {EventName.ModLoaded, EventPermission.Transparent},
            {EventName.PlayerHurt, EventPermission.ListenOnly},
            {EventName.ShowDamage, EventPermission.Transparent},
            {EventName.ShowHealth, EventPermission.Transparent},
            {EventName.ShowInventory, EventPermission.Transparent},
            {EventName.SwitchScene, EventPermission.Transparent},
            {EventName.TaskComplete, EventPermission.ListenOnly},
            {EventName.Tick, EventPermission.Transparent},
        };
	}

	//Register an event listener
 	public static void AddListener<T>(string name, Action<T> listener) {
        if(!instance.events.ContainsKey(name))
			instance.events[name] = null;
        if(instance.events[name] != null && instance.events[name].GetType() != typeof(Action<T>))
            throw new InvalidCastException("Event " + name + " has already registered a different type of callback.");
        instance.events[name] = (Action<T>)instance.events[name] + listener;
    }
    public static void AddListener<T>(EventName eventName, Action<T> listener) {
        AddListener(eventName.ToString(), listener);
    }

	//Remove an event listener
    public static void RemoveListener<T>(string name, Action<T> listener) {
        if(instance.events.ContainsKey(name)) {
            if (instance.events[name].GetType() != typeof(Action<T>))
                throw new InvalidCastException("Event " + name + " has already registered a different type of callback.");
            instance.events[name] = (Action<T>)instance.events[name] - listener;
        }
    }
    public static void RemoveListener<T>(EventName eventName, Action<T> listener) {
        RemoveListener(eventName.ToString(), listener);
    }

	//Trigger an event
	public static void TriggerEvent<T>(string name, T message) {
        if(instance.events.ContainsKey(name)) {
			Action<T> action = instance.events[name] as Action<T>;
            if (action != null) action.Invoke(message);
            else throw new InvalidCastException("Event " + name + " has already registered a different type of callback.");
        }
    }
    public static void TriggerEvent<T>(EventName eventName, T message) {
        TriggerEvent(eventName.ToString(), message);
    }

    public static EventPermission GetEventPermission(string name) {
        if(Enum.IsDefined(typeof(EventName), name))
            return instance.eventPermission[(EventName)Enum.Parse(typeof(EventName), name)];
        return EventPermission.FullAccess;
    }
    public static EventPermission GetEventPermission(EventName eventName) {
        return GetEventPermission(eventName.ToString());
    }
}
