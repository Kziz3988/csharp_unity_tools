using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

public class JsonUpdater {
	[MenuItem("Tools/Update JSON Files/All")]
	public static void UpdateAllJsons() {
		UpdateBulletDataJsons();
		UpdateEnemyDataJsons();
		UpdateEnemyWeaponDataJsons();
		UpdateLaserDataJsons();
		UpdateLevelDataJsons();
		UpdatePlayerDataJsons();
		UpdatePlayerWeaponDataJsons();
		UpdateSectorDataJsons();
		UpdateTaskDataJsons();
	}
	[MenuItem("Tools/Update JSON Files/Bullets")]
	public static void UpdateBulletDataJsons() {
		UpdateJsons<BulletData>("Assets/Assets/DataAssets/Bullet");
	}
	[MenuItem("Tools/Update JSON Files/Enemy Ships")]
	public static void UpdateEnemyDataJsons() {
		UpdateJsons<EnemyData>("Assets/Assets/DataAssets/EnemyShip");
	}
	[MenuItem("Tools/Update JSON Files/Enemy Weapons")]
	public static void UpdateEnemyWeaponDataJsons() {
		UpdateJsons<EnemyWeaponData>("Assets/Assets/DataAssets/EnemyWeapon");
	}
	[MenuItem("Tools/Update JSON Files/Lasers")]
	public static void UpdateLaserDataJsons() {
		UpdateJsons<LaserData>("Assets/Assets/DataAssets/Laser");
	}
	[MenuItem("Tools/Update JSON Files/Levels")]
	public static void UpdateLevelDataJsons() {
		UpdateJsons<LevelData>("Assets/Assets/DataAssets/Level");
	}
	[MenuItem("Tools/Update JSON Files/Player Ships")]
	public static void UpdatePlayerDataJsons() {
		UpdateJsons<PlayerData>("Assets/Assets/DataAssets/PlayerShip");
	}
	[MenuItem("Tools/Update JSON Files/Player Weapons")]
	public static void UpdatePlayerWeaponDataJsons() {
		UpdateJsons<PlayerWeaponData>("Assets/Assets/DataAssets/PlayerWeapon");
	}
	[MenuItem("Tools/Update JSON Files/Sectors")]
	public static void UpdateSectorDataJsons() {
		UpdateJsons<SectorData>("Assets/Assets/DataAssets/Sector");
	}
	[MenuItem("Tools/Update JSON Files/Tasks")]
	public static void UpdateTaskDataJsons() {
		UpdateJsons<TaskData>("Assets/Assets/DataAssets/Task");
	}

	static readonly JsonSerializer serializer = JsonSerializer.Create(JsonUtil.defaultSettings);
	
	public static void UpdateJsons<T>(string folderPath) where T : new() {
        if(!Directory.Exists(folderPath)) {
            Debug.LogError("Folder not found: " + folderPath);
            return;
        }
        string[] files = Directory.GetFiles(folderPath, "*.json", SearchOption.TopDirectoryOnly);
        foreach(string file in files) {
            try {
                string jsonText = File.ReadAllText(file);
				JObject jObject;
				try {
					jObject = JObject.Parse(jsonText);
				}
                catch(Exception) {
					jObject = new JObject();
				}
                JObject fixedJObject = FixJsonObject(jObject, typeof(T));
                string updatedJson = JsonConvert.SerializeObject(fixedJObject, Formatting.Indented, JsonUtil.defaultSettings);
                File.WriteAllText(file, updatedJson);
                Debug.Log("Successfully updated JSON: " + file);
            }
            catch(Exception e) {
                Debug.LogError("Failed to update JSON " + file + ":" + e);
            }
        }
		AssetDatabase.Refresh();
    }

	static JObject FixJsonObject(JObject jObject, Type targetType) {
		JObject result = new JObject();
		object defaultInstance = Activator.CreateInstance(targetType);
		foreach(var prop in targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
			if(!prop.CanWrite) continue;
			JToken token;
			object value;
			if (jObject.TryGetValue(prop.Name, StringComparison.OrdinalIgnoreCase, out token))
				value = ConvertTokenToType(token, prop.PropertyType);
			else value = prop.GetValue(defaultInstance, null);
			result[prop.Name] = SafeToJToken(value, serializer);
		}
		foreach(var field in targetType.GetFields(BindingFlags.Public | BindingFlags.Instance)) {
			JToken token;
			object value;
			if (jObject.TryGetValue(field.Name, StringComparison.OrdinalIgnoreCase, out token))
				value = ConvertTokenToType(token, field.FieldType);
			else value = field.GetValue(defaultInstance);
			result[field.Name] = SafeToJToken(value, serializer);
		}
		return result;
	}

	static JToken SafeToJToken(object value, JsonSerializer serializer) {
		if (value == null)
			return JValue.CreateNull();
		return JToken.FromObject(value, serializer);
	}

    static object ConvertTokenToType(JToken token, Type targetType) {
        try {
            if(token.Type == JTokenType.Null) return null;
            if(targetType.IsClass && targetType != typeof(string)) {
                if(token is JObject) return FixJsonObject((JObject)token, targetType).ToObject(targetType, serializer);
            }
            if(targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Dictionary<,>)) {
                Type keyType = targetType.GetGenericArguments()[0];
                Type valueType = targetType.GetGenericArguments()[1];
                if(token is JObject) {
                    var dict = (IDictionary)Activator.CreateInstance(targetType);
                    foreach(var kvp in (JObject)token) {
                        object key = Convert.ChangeType(kvp.Key, keyType);
                        object value = ConvertTokenToType(kvp.Value, valueType);
                        dict.Add(key, value);
                    }
                    return dict;
                }
            }
            if(targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>)) {
                Type elemType = targetType.GetGenericArguments()[0];
                if(token is JArray) {
                    IList list = (IList)Activator.CreateInstance(targetType);
                    foreach(var item in (JArray)token) {
                        list.Add(ConvertTokenToType(item, elemType));
                    }
                    return list;
                }
            }
            return token.ToObject(targetType, serializer);
        }
        catch {
    		return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
    }
}
