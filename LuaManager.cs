using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

public class LuaManager : MonoBehaviour {
	public static LuaManager instance;
	static readonly Dictionary<Identifier, Script> luaScripts = new Dictionary<Identifier, Script>();

	void Awake() {
		if(instance == null) {
			instance = this;
			luaScripts.Clear();
		}
		else Destroy(gameObject);
	}

	public static IEnumerator LoadScript(Identifier id) {
		if(luaScripts.ContainsKey(id)) yield break;
		Script script = LuaUtility.CreateDefaultScript();
		luaScripts[id] = script;
		yield return ModLoader.LoadTextAsync(ModData.GetDefaultModPath(id.attribution), id.ParseEntryName("Lua", ".lua"), data => {
			try{
				luaScripts[id].DoString(data);
			}
			catch(System.Exception ex) {
				Debug.LogError("The Lua script contains an error, The default behavior is now being used: " + ex.Message);
			}
		});
	}

	public static DynValue GetFunction(Identifier id, string name) {
		if(luaScripts.ContainsKey(id))
			return luaScripts[id].Globals.Get(name);
		else {
			Debug.LogError("[Lua Script Manager] Lua script \"" + id.ToString() + "\" has not loaded yet!");
			return null;
		}
	}

	public static DynValue CallFunction(Identifier id, DynValue function, params object[] args) {
		if(luaScripts.ContainsKey(id)){
			return luaScripts[id].Call(function, args);
		}
		else {
			Debug.LogError("[Lua Script Manager] Lua script \"" + id.ToString() + "\" has not loaded yet!");
			return null;
		}
	}
}
