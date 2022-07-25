using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using game.GameEngine.Components;
using Action = game.GameEngine.Components.Action;

namespace game.GameEngine
{
    [JsonConverter(typeof(ComponentConverter))]
    public class Component
    {
        public Component()
        {
                
        }
    }

    public class ComponentConverter : JsonConverter<Component>
    {
        public override Component? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            
            reader.Read();
            reader.Read();
            var type = reader.GetString();
            reader.Read();
            
            if (type is null)
                throw new Exception("No component type in json");
            
            var jsonDocument = JsonDocument.ParseValue(ref reader);
            dynamic component = jsonDocument.Deserialize(Type.GetType(type));
            reader.Read();
            return component;
        }

        public override void Write(Utf8JsonWriter writer, Component value, JsonSerializerOptions options)
        {
            var type = value.GetType();
            writer.WriteStartObject();
            writer.WriteString("Type",type.FullName);
            writer.WriteStartObject("Values");
            foreach (var propertyInfo in type.GetProperties())
            {
                writer.WritePropertyName(propertyInfo.Name);
                writer.WriteRawValue(JsonSerializer.Serialize(propertyInfo.GetValue(value)),true);
            }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
    }
}