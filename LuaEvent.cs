using System;
using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

public class LuaEvent {
	public void AddListener(string eventName, DynValue luaFunc) {
        EventPermission permission = EventManager.GetEventPermission(eventName);
        if(permission == EventPermission.Transparent) {
            Debug.LogWarning("Lua cannot listen to event \"" + eventName + "\"");
            return;
        }
        if (luaFunc.Type != DataType.Function) {
            Debug.LogError("AddListener expects a function");
            return;
        }
        Action<object> callback = (msg) => {
			string str = (msg != null) ? msg.ToString() : "";
    		luaFunc.Function.Call(DynValue.NewString(str));
        };
        EventManager.AddListener(eventName, callback);
    }

    public void RemoveListener(string eventName, DynValue luaFunc) {
        EventPermission permission = EventManager.GetEventPermission(eventName);
        if(permission != EventPermission.FullAccess) {
            Debug.LogWarning("Lua cannot remove listener for event \"" + eventName + "\"");
            return;
        }
        if(luaFunc.Type != DataType.Function) {
            Debug.LogError("RemoveListener expects a function");
            return;
        }
        Action<object> callback = (msg) => {
			string str = (msg != null) ? msg.ToString() : "";
    		luaFunc.Function.Call(DynValue.NewString(str));
        };
        EventManager.RemoveListener(eventName, callback);
    }

    public void TriggerEvent(string eventName, object message) {
        EventPermission permission = EventManager.GetEventPermission(eventName);
        if(permission != EventPermission.ListenTrigger && permission != EventPermission.FullAccess) {
            Debug.LogWarning("Lua cannot trigger event \"" + eventName + "\"");
            return;
        }
        EventManager.TriggerEvent(eventName, message);
    }
}
