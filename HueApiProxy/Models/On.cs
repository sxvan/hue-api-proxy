using System.Text.Json.Serialization;

namespace MyStromButton.Models
{
    public class On
    {
        [JsonPropertyName("on")]
        public bool IsOn { get; set; }
    }
}
