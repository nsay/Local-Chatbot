using LocalChatBotBackend.Interfaces;
using LocalChatBotBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace LocalChatBotBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectAnalyzerService _analyzer;

        public ProjectController(IProjectAnalyzerService analyzer)
        {
            _analyzer = analyzer;
        }

        [HttpPost("content")]
        public async Task<IActionResult> Analyze([FromBody] ProjectRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Path))
                return BadRequest(new { error = "Path is required" });

            if (string.IsNullOrWhiteSpace(request.Type))
                return BadRequest(new { error = "Type is required" });

            var result = await _analyzer.AnalyzeProjectAsync(request.Path, request.Type);
            return Ok(result);
        }
    }

    public class ProjectRequest
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
