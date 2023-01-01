using HueApiProxy.Models;
using Microsoft.AspNetCore.Mvc;
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
            var sceneResponse = await this.http.GetFromJsonAsync<SceneGetResponse>($"https://{this.hueBridgeIp}/clip/v2/resource/scene/{id}");
            var res = sceneResponse.Data[0];

            for (int i = 0; i < res.GetProperty("actions").GetArrayLength(); i++)
            {
                var action = res.GetProperty("actions")[i];
                var executeAction = action.GetProperty("action");
                var rid = action.GetProperty("target").GetProperty("rid").GetString();

                await this.http.PutAsJsonAsync($"https://{this.hueBridgeIp}/clip/v2/resource/light/{rid}", executeAction as object);
            }

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
