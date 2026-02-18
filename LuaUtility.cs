using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

public static class LuaUtility {
	public static Type[] defaultTypes = {
		//Unity structs and classes
		typeof(Vector2),
		typeof(Vector2Int),
		typeof(Vector3),
		typeof(Vector3Int),
		typeof(Vector4),
		typeof(Quaternion),
		typeof(Rect),
		typeof(RectInt),
		typeof(Color),
		typeof(Color32),
		typeof(Mathf),

		//Custom tool classes and interfaces
		typeof(MathUtility),
		typeof(LuaEvent),
		typeof(IBullet),
		typeof(IEnemy),
		typeof(IMovable),
		typeof(IPerceptible),

		//Custom data structures and classes
		typeof(Advancement),
		typeof(AssetData),
		typeof(AttackPattern),
		typeof(BulletData),
		typeof(BulletMovement),
		typeof(BulletData),
		typeof(CriticalDamage),
		typeof(Damage),
		typeof(EnemyData),
		typeof(EnemyNodeData),
		typeof(EnemyWeaponData),
		typeof(Identifier),
		typeof(ItemPosition),
		typeof(LaserData),
		typeof(LevelData),
		typeof(PlayerAttitude),
		typeof(PlayerData),
		typeof(PlayerWeaponData),
		typeof(Range),
		typeof(ReferencePattern),
		typeof(RotationPattern),
		typeof(Sector),
		typeof(SectorData),
		typeof(ShotPattern),
		typeof(ShotType),
		typeof(Storage),
		typeof(TaskData),
		typeof(Wave),
		typeof(WeaponData)
	};

    public static Script CreateDefaultScript() {
        Script lua = new Script(CoreModules.Preset_Complete);
		foreach(Type type in defaultTypes)
			AddType(lua, type, type.Name);
        return lua;
    }

	//Allow access to the type
	public static void AddType(this Script lua, Type type, string globalName = "") {
        UserData.RegisterType(type);
        if (!string.IsNullOrEmpty(globalName))
            lua.Globals[globalName] = UserData.CreateStatic(type);
    }

	//Allow access to the object
	public static void AddObject(this Script lua, object obj, string globalName = "") {
		UserData.RegisterType(obj.GetType());
		if (!string.IsNullOrEmpty(globalName))
            lua.Globals[globalName] = obj;
    }
}
