using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Linq;
using System.Text.Json;

namespace RemiBou.BlogPost.SignalR.Shared
{
  public class NotificationJsonConverter : JsonConverter<SerializedNotification>
  {
    private readonly IEnumerable<Type> _types;

    public NotificationJsonConverter()
    {
      var type = typeof(SerializedNotification);

      // find all SerializedNotification types in the assembly
      _types = AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(s => s.GetTypes())
          .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
          .ToList();
    }

    public override SerializedNotification Read(ref Utf8JsonReader reader,
                                                Type typeToConvert,
                                                JsonSerializerOptions options)
    {
      if (reader.TokenType != JsonTokenType.StartObject)
      {
        throw new JsonException();
      }

      using (var jsonDocument = JsonDocument.ParseValue(ref reader))
      {
        // find the property with the name of the notification type
        if (!jsonDocument.RootElement.TryGetProperty("notificationType", out var typeProperty))
        {
          throw new JsonException();
        }

        // using the name of the type find the type
        var type = _types.FirstOrDefault(x => x.Name == typeProperty.GetString());

        if (type == null)
        {
          throw new JsonException();
        }

        // convert json text to the found type
        var jsonObject = jsonDocument.RootElement.GetRawText();
        var result = (SerializedNotification)JsonSerializer.Deserialize(jsonObject, type, options);

        return result;
      }
    }

    public override void Write(Utf8JsonWriter writer,
                               SerializedNotification value,
                               JsonSerializerOptions options)
    {
      JsonSerializer.Serialize(writer, value, options);
    }
  }
}
