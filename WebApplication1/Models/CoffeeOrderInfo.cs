using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
        Awaiting,
        Preparing,
        Ready,
        Created
    }

    public class CoffeeOrderInfo
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatus Status { get; set; }
        public CoffeeSettings? CoffeeSettings { get; set; }


    }

}
