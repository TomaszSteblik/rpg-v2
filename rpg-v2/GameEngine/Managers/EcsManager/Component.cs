using System.Text.Json.Serialization;
using game.GameEngine.Shared;

namespace game.GameEngine
{
    [JsonConverter(typeof(SerializationSomConverter<Component>))]
    public class Component
    {
        public Component()
        {

        }
    }
}