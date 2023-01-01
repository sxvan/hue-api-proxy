using System.Text.Json;

namespace HueApiProxy.Models
{
    public class Test
    {
        public object[] Errors { get; set; }

        public JsonElement[] Data { get; set; }
    }
}
