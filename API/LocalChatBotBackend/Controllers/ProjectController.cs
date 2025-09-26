using LocalChatBotBackend.Interfaces;
using LocalChatBotBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using LocalChatBotBackend.Models;

namespace LocalChatBotBackend.Controllers
{
    /// <summary>
    /// Controller responsible for handling project-related requests
    /// Provides endpoints for analyzing projects and returning their JSON representation
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        // Service for analyzing projects
        private readonly IProjectAnalyzerService _analyzer;

        public ProjectController(IProjectAnalyzerService analyzer)
        {
            _analyzer = analyzer;
        }

        /// <summary>
        /// POST endpoint to analyze a project and return its JSON content
        /// </summary>
        /// <param name="request">Contains project path and type</param>
        /// <returns>Analyzed project JSON or error message</returns>
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
}
