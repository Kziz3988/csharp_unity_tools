using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class UnityJsonConverters {
    public static JsonConverter[] converters = {
        new Vector2ShorterJsonConverter(),
        new Vector2IntShorterJsonConverter(),
        new Vector3ShorterJsonConverter(),
        new Vector3IntShorterJsonConverter(),
        new Vector4ShorterJsonConverter(),
        new QuaternionShorterJsonConverter(),
        new RectShorterJsonConverter(),
        new RectIntShorterJsonConverter(),
        new ColorShorterJsonConverter(),
        new Color32ShorterJsonConverter()
    };
}

public class Vector2JsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Vector2);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JObject obj = JObject.Load(reader);
        return new Vector2(
            (float)obj["x"],
            (float)obj["y"]
        );
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Vector2 v = (Vector2)value;
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(v.x);
        writer.WritePropertyName("y");
        writer.WriteValue(v.y);
        writer.WriteEndObject();
    }
}

public class Vector2IntJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Vector2Int);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JObject obj = JObject.Load(reader);
        return new Vector2Int(
            (int)obj["x"],
            (int)obj["y"]
        );
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Vector2Int v = (Vector2Int)value;
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(v.x);
        writer.WritePropertyName("y");
        writer.WriteValue(v.y);
        writer.WriteEndObject();
    }
}

public class Vector3JsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Vector3);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JObject obj = JObject.Load(reader);
        return new Vector3(
            (float)obj["x"],
            (float)obj["y"],
            (float)obj["z"]
        );
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Vector3 v = (Vector3)value;
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(v.x);
        writer.WritePropertyName("y");
        writer.WriteValue(v.y);
        writer.WritePropertyName("z");
        writer.WriteValue(v.z);
        writer.WriteEndObject();
    }
}

public class Vector3IntJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Vector3Int);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JObject obj = JObject.Load(reader);
        return new Vector3Int(
            (int)obj["x"],
            (int)obj["y"],
            (int)obj["z"]
        );
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Vector3Int v = (Vector3Int)value;
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(v.x);
        writer.WritePropertyName("y");
        writer.WriteValue(v.y);
        writer.WritePropertyName("z");
        writer.WriteValue(v.z);
        writer.WriteEndObject();
    }
}

public class Vector4JsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Vector4);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JObject obj = JObject.Load(reader);
        return new Vector4(
            (float)obj["x"],
            (float)obj["y"],
            (float)obj["z"],
            (float)obj["w"]
        );
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Vector4 v = (Vector4)value;
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(v.x);
        writer.WritePropertyName("y");
        writer.WriteValue(v.y);
        writer.WritePropertyName("z");
        writer.WriteValue(v.z);
        writer.WritePropertyName("w");
        writer.WriteValue(v.w);
        writer.WriteEndObject();
    }
}

public class QuaternionJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Quaternion);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JObject obj = JObject.Load(reader);
        return new Quaternion(
            (float)obj["x"],
            (float)obj["y"],
            (float)obj["z"],
            (float)obj["w"]
        );
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Quaternion q = (Quaternion)value;
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(q.x);
        writer.WritePropertyName("y");
        writer.WriteValue(q.y);
        writer.WritePropertyName("z");
        writer.WriteValue(q.z);
        writer.WritePropertyName("w");
        writer.WriteValue(q.w);
        writer.WriteEndObject();
    }
}

public class RectJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Rect);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JObject obj = JObject.Load(reader);
        return new Rect(
            (float)obj["x"],
            (float)obj["y"],
            (float)obj["width"],
            (float)obj["height"]
        );
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Rect r = (Rect)value;
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(r.x);
        writer.WritePropertyName("y");
        writer.WriteValue(r.y);
        writer.WritePropertyName("width");
        writer.WriteValue(r.width);
        writer.WritePropertyName("height");
        writer.WriteValue(r.height);
        writer.WriteEndObject();
    }
}

public class RectIntJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(RectInt);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JObject obj = JObject.Load(reader);
        return new RectInt(
            (int)obj["x"],
            (int)obj["y"],
            (int)obj["width"],
            (int)obj["height"]
        );
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        RectInt r = (RectInt)value;
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(r.x);
        writer.WritePropertyName("y");
        writer.WriteValue(r.y);
        writer.WritePropertyName("width");
        writer.WriteValue(r.width);
        writer.WritePropertyName("height");
        writer.WriteValue(r.height);
        writer.WriteEndObject();
    }
}

public class ColorJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Color);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JObject obj = JObject.Load(reader);
        return new Color(
            (float)obj["r"],
            (float)obj["g"],
            (float)obj["b"],
            (float)obj["a"]
        );
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Color c = (Color)value;
        writer.WriteStartObject();
        writer.WritePropertyName("r");
        writer.WriteValue(c.r);
        writer.WritePropertyName("g");
        writer.WriteValue(c.g);
        writer.WritePropertyName("b");
        writer.WriteValue(c.b);
        writer.WritePropertyName("a");
        writer.WriteValue(c.a);
        writer.WriteEndObject();
    }
}

public class Color32JsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Color32);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JObject obj = JObject.Load(reader);
        return new Color32(
            (byte)obj["r"],
            (byte)obj["g"],
            (byte)obj["b"],
            (byte)obj["a"]
        );
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Color32 c = (Color32)value;
        writer.WriteStartObject();
        writer.WritePropertyName("r");
        writer.WriteValue(c.r);
        writer.WritePropertyName("g");
        writer.WriteValue(c.g);
        writer.WritePropertyName("b");
        writer.WriteValue(c.b);
        writer.WritePropertyName("a");
        writer.WriteValue(c.a);
        writer.WriteEndObject();
    }
}

public class Vector2ShorterJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Vector2);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JArray arr = JArray.Load(reader);
        return new Vector2((float)arr[0], (float)arr[1]);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Vector2 v = (Vector2)value;
        writer.WriteStartArray();
        writer.WriteValue(v.x);
        writer.WriteValue(v.y);
        writer.WriteEndArray();
    }
}

public class Vector2IntShorterJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Vector2Int);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JArray arr = JArray.Load(reader);
        return new Vector2Int((int)arr[0], (int)arr[1]);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Vector2Int v = (Vector2Int)value;
        writer.WriteStartArray();
        writer.WriteValue(v.x);
        writer.WriteValue(v.y);
        writer.WriteEndArray();
    }
}

public class Vector3ShorterJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Vector3);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JArray arr = JArray.Load(reader);
        return new Vector3((float)arr[0], (float)arr[1], (float)arr[2]);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Vector3 v = (Vector3)value;
        writer.WriteStartArray();
        writer.WriteValue(v.x);
        writer.WriteValue(v.y);
        writer.WriteValue(v.z);
        writer.WriteEndArray();
    }
}

public class Vector3IntShorterJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Vector3Int);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JArray arr = JArray.Load(reader);
        return new Vector3Int((int)arr[0], (int)arr[1], (int)arr[2]);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Vector3Int v = (Vector3Int)value;
        writer.WriteStartArray();
        writer.WriteValue(v.x);
        writer.WriteValue(v.y);
        writer.WriteValue(v.z);
        writer.WriteEndArray();
    }
}

public class Vector4ShorterJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Vector4);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JArray arr = JArray.Load(reader);
        return new Vector4((float)arr[0], (float)arr[1], (float)arr[2], (float)arr[3]);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Vector4 v = (Vector4)value;
        writer.WriteStartArray();
        writer.WriteValue(v.x);
        writer.WriteValue(v.y);
        writer.WriteValue(v.z);
        writer.WriteValue(v.w);
        writer.WriteEndArray();
    }
}

public class QuaternionShorterJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Quaternion);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JArray arr = JArray.Load(reader);
        return new Quaternion((float)arr[0], (float)arr[1], (float)arr[2], (float)arr[3]);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Quaternion q = (Quaternion)value;
        writer.WriteStartArray();
        writer.WriteValue(q.x);
        writer.WriteValue(q.y);
        writer.WriteValue(q.z);
        writer.WriteValue(q.w);
        writer.WriteEndArray();
    }
}

public class RectShorterJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Rect);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JArray arr = JArray.Load(reader);
        return new Rect((float)arr[0], (float)arr[1], (float)arr[2], (float)arr[3]);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Rect r = (Rect)value;
        writer.WriteStartArray();
        writer.WriteValue(r.x);
        writer.WriteValue(r.y);
        writer.WriteValue(r.width);
        writer.WriteValue(r.height);
        writer.WriteEndArray();
    }
}

public class RectIntShorterJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(RectInt);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JArray arr = JArray.Load(reader);
        return new RectInt((int)arr[0], (int)arr[1], (int)arr[2], (int)arr[3]);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        RectInt r = (RectInt)value;
        writer.WriteStartArray();
        writer.WriteValue(r.x);
        writer.WriteValue(r.y);
        writer.WriteValue(r.width);
        writer.WriteValue(r.height);
        writer.WriteEndArray();
    }
}

public class ColorShorterJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Color);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JArray arr = JArray.Load(reader);
        return new Color((float)arr[0], (float)arr[1], (float)arr[2], (float)arr[3]);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Color c = (Color)value;
        writer.WriteStartArray();
        writer.WriteValue(c.r);
        writer.WriteValue(c.g);
        writer.WriteValue(c.b);
        writer.WriteValue(c.a);
        writer.WriteEndArray();
    }
}

public class Color32ShorterJsonConverter : JsonConverter {
    public override bool CanConvert(Type objectType) {
        return objectType == typeof(Color32);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JArray arr = JArray.Load(reader);
        return new Color32((byte)arr[0], (byte)arr[1], (byte)arr[2], (byte)arr[3]);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        Color32 c = (Color32)value;
        writer.WriteStartArray();
        writer.WriteValue(c.r);
        writer.WriteValue(c.g);
        writer.WriteValue(c.b);
        writer.WriteValue(c.a);
        writer.WriteEndArray();
    }
}
