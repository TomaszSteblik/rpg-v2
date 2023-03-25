using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace game.GameEngine.GameObjects.States;

[JsonConverter(typeof(StateDataConverter))]
public abstract class StateData
{
    public abstract string OwnerType { get; }
    public StateStatus Status { get; set; }
}

public enum StateStatus
{
    Created,
    Active,
    Destructed
}

public class StateDataConverter : JsonConverter<StateData>
{
    public override StateData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

    public override void Write(Utf8JsonWriter writer, StateData value, JsonSerializerOptions options)
    {
        var type = value.GetType();
        writer.WriteStartObject();
        writer.WriteString("Type", type.FullName);
        writer.WriteStartObject("Values");
        foreach (var propertyInfo in type.GetProperties())
        {
            writer.WritePropertyName(propertyInfo.Name);
            writer.WriteRawValue(JsonSerializer.Serialize(propertyInfo.GetValue(value)), true);
        }
        writer.WriteEndObject();
        writer.WriteEndObject();
    }
}