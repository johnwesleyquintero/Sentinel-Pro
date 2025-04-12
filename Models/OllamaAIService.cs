using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace WorkspaceCleanup.Models
{
    /// <summary>
    /// Implementation of IAIService using Ollama for AI operations.
    /// Provides integration with local AI models for code assistance and natural language processing.
    /// </summary>
    public class OllamaAIService : IAIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly JsonSerializerOptions _jsonOptions;
        private bool _isInitialized;

        /// <summary>
        /// Initializes a new instance of the OllamaAIService.
        /// </summary>
        /// <param name="httpClientFactory">The HTTP client factory for making requests to Ollama API.</param>
        /// <param name="configuration">The configuration containing Ollama settings.</param>
        public OllamaAIService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _isInitialized = false;
        }

        /// <summary>
        /// Checks if required models are available in Ollama.
        /// </summary>
        private async Task EnsureModelsAvailableAsync()
        {
            if (_isInitialized) return;

            try
            {
                using var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"{_configuration["OllamaEndpoint"]}/api/tags");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var models = JsonSerializer.Deserialize<OllamaModels>(content, _jsonOptions);

                // Verify required models
                var requiredModels = new[] { "codellama", "mistral" };
                var missingModels = requiredModels.Where(m =>
                    !models.Models.Any(x => x.Name.Contains(m)));

                if (missingModels.Any())
                {
                    throw new InvalidOperationException($"Missing required AI models: {string.Join(", ", missingModels)}");
                }

                _isInitialized = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AI model verification failed");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<string> GetCodeCompletionAsync(string codeContext)
        {
            try
            {
                var response = await SendOllamaRequestAsync("codellama", codeContext);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting code completion");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<string> GetCodeExplanationAsync(string code)
        {
            try
            {
                var prompt = $"Explain this code:\n{code}";
                var response = await SendOllamaRequestAsync("mistral", prompt);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting code explanation");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<string> GetNaturalLanguageResponseAsync(string query)
        {
            try
            {
                var response = await SendOllamaRequestAsync("mistral", query);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting natural language response");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<string> DetectCodeErrorsAsync(string code)
        {
            try
            {
                var prompt = $"Analyze this code for potential errors and issues:\n{code}";
                var response = await SendOllamaRequestAsync("codellama", prompt);
                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error detecting code errors");
                throw;
            }
        }

        /// <summary>
        /// Sends a request to the Ollama API and returns the response.
        /// </summary>
        /// <param name="model">The name of the AI model to use (e.g., "codellama", "mistral").</param>
        /// <param name="prompt">The prompt to send to the model.</param>
        /// <returns>The model's response as a string.</returns>
        private async Task<string> SendOllamaRequestAsync(string model, string prompt)
        {
            await EnsureModelsAvailableAsync();

            var request = new
            {
                model = model,
                prompt = prompt,
                stream = false
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request, _jsonOptions),
                Encoding.UTF8,
                "application/json");

            using var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync($"{_configuration["OllamaEndpoint"]}/api/generate", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<OllamaResponse>(responseContent, _jsonOptions);

            return result?.Response ?? string.Empty;
        }

        /// <summary>
        /// Represents the response structure from the Ollama API.
        /// </summary>
        private class OllamaResponse
        {
            /// <summary>
            /// Gets or sets the response text from the AI model.
            /// </summary>
            public string Response { get; set; } = string.Empty;
        }
    }
}


/// <summary>
/// Represents the collection of available Ollama models.
/// </summary>
public class OllamaModels
{
    public List<OllamaModel> Models { get; set; } = new();
}

/// <summary>
/// Represents an individual Ollama model.
/// </summary>
public class OllamaModel
{
    public string Name { get; set; } = string.Empty;
}