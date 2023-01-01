using System.Text.Json;

namespace HueApiProxy.Models
{
    public class SceneGetResponse
    {
        public object[] Errors { get; set; }

        public JsonElement[] Data { get; set; }
    }
}
