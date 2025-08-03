using External_API_Integration.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace External_API_Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionsController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public PredictionsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");
            var content = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, content);
        }

        [HttpPost("url")]
        public async Task<IActionResult> PredictFromUrl([FromBody] string url)
        {
            var payload = new Payload { ImageUrl = url };
            var response = await _httpClient.PostAsJsonAsync("https://fruit-classifier.azurewebsites.net/predict/url", payload);
            var content = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, content);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> PredictFromImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            using var content = new MultipartFormDataContent
            {
                { new StreamContent(stream), "file", file.FileName }
            };

            var response = await _httpClient.PostAsync("https://fruit-classifier.azurewebsites.net/predict/upload", content);
            var result = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, result);
        }
    }
}
