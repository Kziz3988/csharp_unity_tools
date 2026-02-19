Some general-purpose tool scripts. Most of them support native Unity 2017.4+, while a few require the use of [Newtonsoft.Json](https://github.com/SaladLab/Json.Net.Unity3D), [MoonSharp](https://github.com/moonsharp-devs/moonsharp) or [SharpZipLib](https://github.com/icsharpcode/SharpZipLib).

Use them freely.

<table>
  <thead>
    <tr>
      <th align="center"><b>Script</b></th>
      <th align="center"><b>Description</b></th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>EventManager.cs</td>
      <td>A game event manager based on C# delegates. Only the passing of one event argument is supported. Please use custom data structures to pass multiple arguments.</td>
    </tr>
    <tr>
      <td>JsonUpdate.cs</td>
      <td>An editor script that automatically completes missing fields and removes redundant fields in JSON files. Supports complex nested types.</td>
    </tr>
    <tr>
      <td>LocalizationConverter.cs</td>
      <td>Split a localization CSV file into several single-language JSON files.</td>
    </tr>
    <tr>
      <td>LuaEvent.cs</td>
      <td>In conjunction with EventManager, it allows for the listening or triggering of events within Lua scripts, primarily for use in permission management.</td>
    </tr>
    <tr>
      <td>LuaUtility.cs</td>
      <td>Used for quickly creating MoonSharp Script objects. It allows for easy implementation of a LuaManager script, facilitating the implementation of mod systems, etc.</td>
    </tr>
    <tr>
      <td>ModLoader.cs</td>
      <td>Used to read text, textures, etc. from ZIP files. Can be executed asynchronously based on Unity coroutines. It includes a simple cache to improve performance.</td>
    </tr>
    <tr>
      <td>ObjectPool.cs</td>
      <td>A very lightweight object pool, essentially a queue of GameObjects. If an object come from a combination of multiple pools, it needs to be split and released separately.</td>
    </tr>
      <td>PriorityQueue.cs</td>
      <td>A priority queue that supports generics and ascending/descending order. Custom priority values ​​are allowed.</td>
    </tr>
    </tr>
      <td>UnityJsonConverter.cs</td>
      <td>Newtonsoft.json converters for common Unity structures, such as Vector2, Vector3, Quaternion, Rect, Color, etc. It supports both JArray and JObject serialization methods. Suitable as the default configuration for a JsonManager script.</td>
    </tr>
  </tbody>
</table>
