using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class LocalizationConverter : EditorWindow {
    TextAsset csvFile;
    string outputFolder = "Assets/Resources/Languages";

    [MenuItem("Tools/Localization/Convert CSV to JSON files")]
    public static void ShowWindow() {
        GetWindow<LocalizationConverter>("CSV to JSON Converter");
    }

    void OnGUI() {
        GUILayout.Label("CSV to JSON Localization Converter", EditorStyles.boldLabel);
        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV File", csvFile, typeof(TextAsset), false);
        outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);

        if (GUILayout.Button("Convert")) {
            if (csvFile == null) {
                EditorUtility.DisplayDialog("Error", "Please select a CSV file!", "OK");
                return;
            }
            ConvertCSVToJSON(csvFile.text, outputFolder);
            EditorUtility.DisplayDialog("Success", "CSV converted to JSON successfully!", "OK");
        }
    }

    public static void ConvertCSVToJSON(string csvText, string outputFolder) {
        string[] lines = csvText.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2) {
            Debug.LogError("CSV must contain header and at least one data row");
            return;
        }

        //Parsing headers
        string[] headers = lines[0].Split(',');
        int languageCount = headers.Length - 1; //The first column is text key
        var languageDictionaries = new Dictionary<string, Dictionary<string, string>>();

		//Create a dictionary for each language
        for (int i = 1; i < headers.Length; i++) {
            string lang = headers[i].Trim();
            if (!languageDictionaries.ContainsKey(lang))
                languageDictionaries[lang] = new Dictionary<string, string>();
        }
        for (int i = 1; i < lines.Length; i++) {
            string[] cols = lines[i].Split(',');
            if (cols.Length < 2) continue;

            string key = cols[0].Trim();
            for (int j = 1; j < headers.Length && j < cols.Length; j++) {
                string lang = headers[j].Trim();
                string value = cols[j].Trim().Replace("\\n", "\n");
				if (string.IsNullOrEmpty(value)) {
					Debug.LogWarning("Skipped empty value for key \"" + key + "\" in language \"" + lang + "\"");
					continue;
				}
                if (!string.IsNullOrEmpty(key))
                    languageDictionaries[lang][key] = value;
            }
        }

        //Output to JSON file
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);
        foreach (var pair in languageDictionaries) {
            string lang = pair.Key;
            string jsonPath = Path.Combine(outputFolder, lang + ".json");
            JsonUtil.SaveToJson(jsonPath, pair.Value);
            Debug.Log("Exported " + lang + " localization file to " + jsonPath);
        }

        AssetDatabase.Refresh();
    }
}
