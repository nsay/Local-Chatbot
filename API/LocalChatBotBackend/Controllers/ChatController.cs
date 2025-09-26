using LocalChatBotBackend.Models;
using Microsoft.AspNetCore.Mvc;
using OllamaSharp;
using OllamaSharp.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LocalChatBotBackend.Controllers
{
    /// <summary>
    /// Controller responsible for handling chat requests to the LLM (Ollama)
    /// Provides endpoints for sending user questions and receiving responses.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        // Ollama API client for communicating with the local LLM server
        private readonly OllamaApiClient _client;

        public ChatController()
        {
            _client = new OllamaApiClient("http://localhost:11434");
        }

        /// <summary>
        /// POST endpoint to ask a question to the LLM.
        /// Optionally includes full project JSON for context.
        /// </summary>
        /// <param name="request">Contains user's question, project JSON, and context flag</param>
        /// <returns>LLM response in JSON { response: string }</returns>
        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            // Validate that the user provided a question
            if (string.IsNullOrWhiteSpace(request.UserQuestion))
                return BadRequest("UserQuestion is required");

            string prompt;

            // If project context should be included, prepend instructions and project code
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
                // General chat without project context
                prompt = request.UserQuestion;
            }

            // Request object for the LLM
            var generateRequest = new GenerateRequest
            {
                Model = "llama3.2", // Specify the model to use...change this if user has different models downloaded via Ollama
                Prompt = prompt
            };

            try
            {
                string responseText = "";
                // Now we send that generate request to the LLM
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

}
