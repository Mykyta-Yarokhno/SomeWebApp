using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CoffeeMachineState
    {
        On,
        Off
    }

    public class CoffeeMachineInfo
    {
       
        public CoffeeMachineState State { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
