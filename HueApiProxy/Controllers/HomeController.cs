using Microsoft.AspNetCore.Mvc;
using MyStromButton.Models;

namespace MyStromButton.Controllers
{
    [Route("/api/v1")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly string hueBridgeIp;
        private readonly string hueApplicationKey;

        public HomeController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.hueBridgeIp = this.configuration.GetValue<string>("hue.bridge.ip");
            this.hueApplicationKey = this.configuration.GetValue<string>("api.hue.application-key");
        }

        [HttpGet("toggle/{id}")]
        public async Task<IActionResult> Toggle(string id)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback += (a, b, c, d) => true;

            var http = new HttpClient(handler);
            http.DefaultRequestHeaders.Add("hue-application-key", this.hueApplicationKey);

            var lightGetResponse = await http.GetFromJsonAsync<LightGetResponse>($"https://{this.hueBridgeIp}/clip/v2/resource/light/{id}");

            var body = new
            {
                on = new
                {
                    on = !lightGetResponse?.Data.FirstOrDefault()?.On.IsOn
                }
            };

            await http.PutAsJsonAsync($"https://{this.hueBridgeIp}/clip/v2/resource/light/{id}", body);

            return Ok();
        }

        [HttpGet("overview/lights")]
        public async Task<IActionResult> Lights()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback += (a, b, c, d) => true;

            var http = new HttpClient(handler);
            http.DefaultRequestHeaders.Add("hue-application-key", this.hueApplicationKey);

            var lightsResponse = await http.GetFromJsonAsync<object>($"https://{this.hueBridgeIp}/clip/v2/resource/light");

            return Ok(lightsResponse);
        }

        [HttpGet("overview/scenes")]
        public async Task<IActionResult> Scenes()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback += (a, b, c, d) => true;

            var http = new HttpClient(handler);
            http.DefaultRequestHeaders.Add("hue-application-key", this.hueApplicationKey);

            var scenesResponse = await http.GetFromJsonAsync<object>($"https://{this.hueBridgeIp}/clip/v2/resource/scene");

            return Ok(scenesResponse);
        }
    }
}
