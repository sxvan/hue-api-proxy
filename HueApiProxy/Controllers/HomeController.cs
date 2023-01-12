using HueApiProxy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MyStromButton.Models;

namespace MyStromButton.Controllers
{
    [Route("/api/v1")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly HttpClient http;
        private readonly string hueBridgeIp;

        public HomeController(IConfiguration configuration)
        {
            this.hueBridgeIp = configuration.GetValue<string>("hue.bridge.ip");

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback += (a, b, c, d) => true;

            this.http = new HttpClient(handler);
            this.http.DefaultRequestHeaders.Add("hue-application-key", configuration.GetValue<string>("api.hue.application-key"));
        }

        [HttpGet("toggle/{id}")]
        public async Task<IActionResult> Toggle(string id)
        {
            var lightGetResponse = await this.http.GetFromJsonAsync<LightGetResponse>($"https://{this.hueBridgeIp}/clip/v2/resource/light/{id}");

            var body = new
            {
                on = new
                {
                    on = !lightGetResponse?.Data.FirstOrDefault()?.On.IsOn
                }
            };

            await this.http.PutAsJsonAsync($"https://{this.hueBridgeIp}/clip/v2/resource/light/{id}", body);

            return Ok();
        }

        [HttpGet("scene/{id}")]
        public async Task<IActionResult> Scene(string id)
        {
            var body = new
            {
                recall = new
                {
                    action = "active"
                }
            };

            await this.http.PutAsJsonAsync<object>($"https://{this.hueBridgeIp}/clip/v2/resource/scene/{id}", body);

            return Ok();
        }

        [HttpGet("dim/{id}/{direction}/{delta}")]
        public async Task<IActionResult> Dim(string id, string direction, int delta)
        {
            var body = new
            {
                dimming_delta = new
                {
                    action = direction,
                    brightness_delta = delta
                }
            };

            await this.http.PutAsJsonAsync<object>($"https://{this.hueBridgeIp}/clip/v2/resource/light/{id}", body);

            return Ok();
        }

        [HttpGet("effect/{id}/{effect}")]
        public async Task<IActionResult> Effect(string id, string effect)
        {
            var body = new
            {
                effects = new
                {
                    effect = effect
                }
            };

            await this.http.PutAsJsonAsync<object>($"https://{this.hueBridgeIp}/clip/v2/resource/light/{id}", body);

            return Ok();
        }

        [HttpGet("overview/lights")]
        public async Task<IActionResult> Lights()
        {
            var lightsResponse = await this.http.GetFromJsonAsync<object>($"https://{this.hueBridgeIp}/clip/v2/resource/light");

            return Ok(lightsResponse);
        }

        [HttpGet("overview/scenes")]
        public async Task<IActionResult> Scenes()
        {
            var scenesResponse = await this.http.GetFromJsonAsync<object>($"https://{this.hueBridgeIp}/clip/v2/resource/scene");

            return Ok(scenesResponse);
        }
    }
}
