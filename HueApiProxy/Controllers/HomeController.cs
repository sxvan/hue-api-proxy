using Microsoft.AspNetCore.Mvc;
using MyStromButton.Models;

namespace MyStromButton.Controllers
{
    [Route("/api/v1")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet("toggle/{id}")]
        public async Task<IActionResult> Toggle(string id)
        {
            string hueBridgeIP = this.configuration.GetValue<string>("hue.bridge.ip");
            string hueApplicationKey = this.configuration.GetValue<string>("api.hue.application-key");

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback += (a, b, c, d) => true;

            var http = new HttpClient(handler);
            http.DefaultRequestHeaders.Add("hue-application-key", hueApplicationKey);

            var lightGetResponse = await http.GetFromJsonAsync<LightGetResponse>($"https://{hueBridgeIP}/clip/v2/resource/light/{id}");

            var body = new
            {
                on = new
                {
                    on = !lightGetResponse?.Data.FirstOrDefault()?.On.IsOn
                }
            };

            await http.PutAsJsonAsync($"https://{hueBridgeIP}/clip/v2/resource/light/{id}", body);

            return Ok();
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            return Ok("Test");
        }
    }
}
