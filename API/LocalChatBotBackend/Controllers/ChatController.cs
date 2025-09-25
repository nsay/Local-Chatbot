using Microsoft.AspNetCore.Mvc;
using OllamaSharp;
using OllamaSharp.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LocalChatBotBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly OllamaApiClient _client;

        public ChatController()
        {
            _client = new OllamaApiClient("http://localhost:11434");
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserQuestion))
                return BadRequest("UserQuestion is required");

            string prompt;

            if (request.UseProjectContext && request.ProjectJson != null)
            {
                prompt = $@"
                        You are an expert software developer and project analyzer.
                        You are given the full source code of a project.
                        Your job is to explain the project in natural language, focusing on:
                        - What the project does
                        - List the main components, services, and modules
                        - How each part relates to each other
                        - Any insights into purpose, structure, and functionality

                        Project Source Code:
                        {request.ProjectJson}

                        User Question:
                        {request.UserQuestion}

                        Answer in detail in natural language:";
            }
            else
            {
                prompt = request.UserQuestion; // general chat
            }

            var generateRequest = new GenerateRequest
            {
                Model = "llama3.2",
                Prompt = prompt
            };

            try
            {
                string responseText = "";
                await foreach (var message in _client.GenerateAsync(generateRequest, CancellationToken.None))
                {
                    if (message?.Response != null)
                        responseText += message.Response;
                }

                return Ok(new { response = responseText });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class ChatRequest
    {
        public string UserQuestion { get; set; }
        public object ProjectJson { get; set; }
        public bool UseProjectContext { get; set; }
    }

}
