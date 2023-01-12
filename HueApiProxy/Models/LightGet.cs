using HueApiProxy.Models;
using System.Text.Json.Serialization;

namespace MyStromButton.Models
{
    public class LightGet
    {
        public On On { get; set; }

        public Dimming Dimming { get; set; }

        [JsonPropertyName("color_temperature")]
        public ColorTemperature ColorTemperature { get; set; }

        public Color Color { get; set; }

        public Gradient Gradient { get; set; }
    }
}
