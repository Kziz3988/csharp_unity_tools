using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip;

public class ModLoader : MonoBehaviour {

    static readonly Dictionary<string, ZipFile> _loadedZips = new Dictionary<string, ZipFile>();
    static readonly Dictionary<string, object> _assetCache = new Dictionary<string, object>();
    static readonly Dictionary<string, byte[]> _binaryCache = new Dictionary<string, byte[]>();

    public class SpriteConfig {
        public float pixelsPerUnit = UIUtility.pixelsPerUnit;
        public FilterMode filterMode = FilterMode.Point;
        public TextureWrapMode wrapMode = TextureWrapMode.Clamp;
        public Vector2 pivot = new Vector2(0.5f, 0.5f);
    }

    public static ZipFile LoadMod(string modName) {
        string key = ModData.GetDefaultModPath(modName);
        List<string> modPaths = ModData.GetModPaths(modName);
        ZipFile zip = null;
        foreach(string modPath in modPaths) {
            zip = LoadZip(modPath, key);
            if(zip != null) break;
        }
        return zip;
    }

    public static ZipFile LoadZip(string zipPath, string key = "") {
        if(key == "") key = zipPath;
        ZipFile zip;
        if(_loadedZips.TryGetValue(key, out zip))
            return zip;

        if(!File.Exists(zipPath)) {
            Debug.LogError("[ModLoader] Mod file not found: " + zipPath);
            return null;
        }

        FileStream fs = File.OpenRead(zipPath);
        zip = new ZipFile(fs);
        _loadedZips[key] = zip;

        //LogEntriesInZipFile(zip);
        Debug.Log("[ModLoader] Mod file loaded: " + zipPath);
        return zip;
    }

    public static byte[] LoadBytes(string zipPath, string entryName) {
        string key = zipPath + "::" + entryName;
        byte[] cached;
        if(_binaryCache.TryGetValue(key, out cached))
            return cached;

        ZipFile zip = LoadMod(zipPath);
        if(zip == null) return null;

        ZipEntry entry = zip.GetEntry(entryName);
        if(entry == null) {
            Debug.LogWarning("[ModLoader] Entry not found: " + entryName + "(" + zipPath + ")");
            return null;
        }

        Stream stream = zip.GetInputStream(entry);
        MemoryStream ms = new MemoryStream();
        byte[] buffer = new byte[4096];
        int bytesRead;
        while((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            ms.Write(buffer, 0, bytesRead);

        byte[] data = ms.ToArray();
        ms.Close();
        stream.Close();

        _binaryCache[key] = data;
        return data;
    }

    public static string LoadText(string zipPath, string entryName) {
        byte[] data = LoadBytes(zipPath, entryName);
        if(data == null) return null;
        return System.Text.Encoding.UTF8.GetString(data);
    }

    public static Texture2D LoadTexture(string zipPath, string entryName) {
        string key = zipPath + "::" + entryName;
        object cachedObj;
        if(_assetCache.TryGetValue(key, out cachedObj))
            return cachedObj as Texture2D;

        byte[] data = LoadBytes(zipPath, entryName);
        if(data == null) return null;

        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        if(!tex.LoadImage(data)) {
            Debug.LogError("[ModLoader] Texture load failed: " + entryName + "(" + zipPath + ")");
            return null;
        }

        tex.name = entryName;
        _assetCache[key] = tex;
        return tex;
    }

    public static Sprite LoadSprite(string zipPath, string entryName, SpriteConfig config = null) {
        string key = zipPath + "::" + entryName;
        object cachedObj;
        if(_assetCache.TryGetValue(key, out cachedObj))
            return cachedObj as Sprite;

        if(config == null) config = new SpriteConfig();
        byte[] data = LoadBytes(zipPath, entryName);
        if (data == null) return null;

        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        if (!tex.LoadImage(data)) {
            Debug.LogError("[ModLoader] Sprite load failed: " + entryName + "(" + zipPath + ")");
            return null;
        }

        tex.name = entryName;
        tex.filterMode = config.filterMode;
        tex.wrapMode = config.wrapMode;

        Rect spriteRect = new Rect(0, 0, tex.width, tex.height);
        Sprite sprite = Sprite.Create(tex, spriteRect, config.pivot, config.pixelsPerUnit);

        _assetCache[key] = sprite;
        return sprite;
    }

    public static T LoadData<T>(string zipPath, string entryName) {
        string key = zipPath + "::" + entryName;
        object cachedObj;
        if(_assetCache.TryGetValue(key, out cachedObj))
            return (T)cachedObj;

        string json = LoadText(zipPath, entryName);
        if(string.IsNullOrEmpty(json)) {
            Debug.LogError("[ModLoader] Failed to load JSON data: " + entryName);
            return default(T);
        }

        T obj = JsonUtil.Parse<T>(json);
        _assetCache[key] = obj;
        return obj;
    }

    public static void UnloadZip(string key) {
        ZipFile zip;
        if (_loadedZips.TryGetValue(key, out zip)) {
            zip.Close();
            _loadedZips.Remove(key);
            Debug.Log("[ModLoader] Mod file unloaded: " + key);
        }
        RemoveCacheByPrefix(key + "::");
    }

    public static void UnloadAll() {
        foreach(KeyValuePair<string, ZipFile> pair in _loadedZips)
            pair.Value.Close();
        _loadedZips.Clear();

        foreach(KeyValuePair<string, object> pair in _assetCache) {
            if(pair.Value is UnityEngine.Object) Destroy((UnityEngine.Object)pair.Value);
        }

        _assetCache.Clear();
        _binaryCache.Clear();

        Debug.Log("[ModLoader] All mod files unloaded");
    }

    static void RemoveCacheByPrefix(string prefix) {
        List<string> keys = new List<string>();

        foreach(KeyValuePair<string, object> pair in _assetCache) {
            if (pair.Key.StartsWith(prefix))
                keys.Add(pair.Key);
        }

        foreach(string k in keys) {
            if(_assetCache[k] is UnityEngine.Object) Destroy((UnityEngine.Object)_assetCache[k]);
            _assetCache.Remove(k);
        }

        keys.Clear();
        foreach(KeyValuePair<string, byte[]> pair in _binaryCache) {
            if(pair.Key.StartsWith(prefix))
                keys.Add(pair.Key);
        }

        foreach(string k in keys)
            _binaryCache.Remove(k);
    }

    public static IEnumerator LoadBytesAsync(string zipPath, string entryName, Action<byte[]> onComplete) {
        string key = zipPath + "::" + entryName;

        if(_binaryCache.ContainsKey(key)) {
            onComplete(_binaryCache[key]);
            yield break;
        }

        ZipFile zip = LoadZip(zipPath);
        if(zip == null) {
            onComplete(null);
            yield break;
        }

        ZipEntry entry = zip.GetEntry(entryName);
        if(entry == null) {
            Debug.LogWarning("[ModLoader] Entry not found: " + entryName);
            onComplete(null);
            yield break;
        }

        Stream stream = zip.GetInputStream(entry);
        MemoryStream ms = new MemoryStream();
        byte[] buffer = new byte[4096];
        int bytesRead = 0;
        int frameGuard = 0;

        while((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0) {
            ms.Write(buffer, 0, bytesRead);
            frameGuard++;
            if(frameGuard >= 8) {
                frameGuard = 0;
                yield return null;
            }
        }

        byte[] data = ms.ToArray();
        ms.Close();
        stream.Close();
        _binaryCache[key] = data;
        onComplete(data);
    }

    public static IEnumerator LoadTextAsync(string zipPath, string entryName, Action<string> onComplete){
        string key = zipPath + "::" + entryName;

        if(_assetCache.ContainsKey(key)) {
            onComplete(_assetCache[key] as string);
            yield break;
        }

        byte[] data = null;
        yield return LoadBytesAsync(zipPath, entryName, d => data = d);

        if(data == null) {
            onComplete(null);
            yield break;
        }

        string text = System.Text.Encoding.UTF8.GetString(data);

        if(string.IsNullOrEmpty(text)) {
            Debug.LogWarning("[ModLoader] Text data is empty: " + entryName + "(" + zipPath + ")");
            onComplete(text);
            yield break;
        }

        _assetCache[key] = text;
        onComplete(text);
    }


    public static IEnumerator LoadTextureAsync(string zipPath, string entryName, Action<Texture2D> onComplete) {
        string key = zipPath + "::" + entryName;

        if(_assetCache.ContainsKey(key)) {
            onComplete((Texture2D)_assetCache[key]);
            yield break;
        }

        byte[] data = null;
        yield return LoadBytesAsync(zipPath, entryName, delegate(byte[] d) { data = d; });

        if(data == null) {
            onComplete(null);
            yield break;
        }

        yield return null;

        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        tex.LoadImage(data);
        tex.name = entryName;
        _assetCache[key] = tex;
        onComplete(tex);
    }

    public static IEnumerator LoadSpriteAsync(string zipPath, string entryName, Action<Sprite> onComplete, SpriteConfig config = null) {
        string key = zipPath + "::" + entryName;

        if(_assetCache.ContainsKey(key)) {
            onComplete((Sprite)_assetCache[key]);
            yield break;
        }

        if(config == null) config = new SpriteConfig();

        Texture2D tex = null;
        yield return LoadTextureAsync(zipPath, entryName, delegate(Texture2D t) { tex = t; });

        if(tex == null) {
            onComplete(null);
            yield break;
        }

        tex.filterMode = config.filterMode;
        tex.wrapMode = config.wrapMode;

        Rect rect = new Rect(0, 0, tex.width, tex.height);
        Sprite sprite = Sprite.Create(tex, rect, config.pivot, config.pixelsPerUnit);
        _assetCache[key] = sprite;
        onComplete(sprite);
    }

    public static IEnumerator LoadDataAsync<T>(string zipPath, string entryName, Action<T> onComplete) {
        string key = zipPath + "::" + entryName;

        if(_assetCache.ContainsKey(key)) {
            onComplete((T)_assetCache[key]);
            yield break;
        }

        byte[] data = null;
        yield return LoadBytesAsync(zipPath, entryName, d => data = d);

        if(data == null) {
            onComplete(default(T));
            yield break;
        }

        string json = System.Text.Encoding.UTF8.GetString(data);

        if(string.IsNullOrEmpty(json)) {
            Debug.LogWarning("[ModLoader] JSON data is empty: " + entryName + "(" + zipPath + ")");
            onComplete(default(T));
            yield break;
        }

        T obj = JsonUtil.Parse<T>(json);
        _assetCache[key] = obj;
        onComplete(obj);
    }

    public static IEnumerator LoadAllDataAsync<T>(string zipPath, string folderEntryName, Action<List<T>> onComplete) {
        List<T> resultList = new List<T>();
        ZipFile zip = LoadZip(zipPath);

        if(zip == null) {
            onComplete(resultList);
            yield break;
        }

        List<ZipEntry> entries = new List<ZipEntry>();
        foreach(ZipEntry entry in zip) {
            if(!entry.IsFile) continue;
            if(entry.Name.StartsWith(folderEntryName)) entries.Add(entry);
        }

        foreach(ZipEntry entry in entries) {
            byte[] data = null;
            yield return LoadBytesAsync(zipPath, entry.Name, d => data = d);

            if(data == null) {
                Debug.LogWarning("[ModLoader] Failed to load file: " + entry.Name);
                continue;
            }

            string json = System.Text.Encoding.UTF8.GetString(data);
            if(string.IsNullOrEmpty(json)) {
                Debug.LogWarning("[ModLoader] JSON data is empty: " + entry.Name);
                continue;
            }

            T obj = JsonUtil.Parse<T>(json);
            string key = zipPath + "::" + entry.Name;
            _assetCache[key] = obj;
            resultList.Add(obj);

            yield return null;
        }

        onComplete(resultList);
    }

    public static void LogLoadingFailed(Identifier id, string type) {
        Debug.Log("[ModLoader] Cannot load asset " + id.ToString() + "(" + type + ")");
    }

    public static void LogEntriesInZipFile(ZipFile zip) {
        Debug.Log("[ModLoader] Listing all entries...");
        foreach(ZipEntry zipEntry in zip)
            Debug.Log(zipEntry.Name);
        Debug.Log("----------");
    }
}
