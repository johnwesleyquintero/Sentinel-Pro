using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.AIPlatform.V1;
using Google.Cloud.AIPlatform.V1;
using Google.Cloud.AIPlatform.V1.Models;

namespace SentinelPro.Models
{
    public class GeminiAIService : IAIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _projectId;
        private readonly JsonSerializerOptions _jsonOptions;

        public GeminiAIService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _apiKey = configuration["GeminiApiKey"];
            _projectId = configuration["GoogleCloudProjectId"];
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new ArgumentException("Gemini API key not found in configuration");
            }
        }

        public async Task<string> GetCodeCompletionAsync(string codeContext)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = $"Complete this code:\n{codeContext}"
                                }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        temperature = 0.7,
                        topK = 40,
                        topP = 0.95,
                        maxOutputTokens = 1024,
                    }
                };

                var response = await client.PostAsync(
                    "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent",
                    new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
                );

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GeminiResponse>(content, _jsonOptions);

                return result?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? string.Empty;
            }
            catch (Exception ex)
            {
                throw new AIServiceException("Error getting code completion from Gemini", ex);
            }
        }

        public async Task<string> GetCodeExplanationAsync(string code)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = $"Explain this code in detail:\n{code}"
                                }
                            }
                        }
                    }
                };

                var response = await client.PostAsync(
                    "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent",
                    new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
                );

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GeminiResponse>(content, _jsonOptions);

                return result?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? string.Empty;
            }
            catch (Exception ex)
            {
                throw new AIServiceException("Error getting code explanation from Gemini", ex);
            }
        }

        public async Task<string> GetNaturalLanguageResponseAsync(string query)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = query
                                }
                            }
                        }
                    }
                };

                var response = await client.PostAsync(
                    "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent",
                    new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
                );

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GeminiResponse>(content, _jsonOptions);

                return result?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? string.Empty;
            }
            catch (Exception ex)
            {
                throw new AIServiceException("Error getting natural language response from Gemini", ex);
            }
        }

        public async Task<string> DetectCodeErrorsAsync(string code)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = $"Analyze this code for potential errors and issues:\n{code}"
                                }
                            }
                        }
                    }
                };

                var response = await client.PostAsync(
                    "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent",
                    new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
                );

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GeminiResponse>(content, _jsonOptions);

                return result?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? string.Empty;
            }
            catch (Exception ex)
            {
                throw new AIServiceException("Error detecting code errors with Gemini", ex);
            }
        }

        private class GeminiResponse
        {
            public Candidate[] Candidates { get; set; }
        }

        private class Candidate
        {
            public Content Content { get; set; }
        }

        private class Content
        {
            public Part[] Parts { get; set; }
        }

        private class Part
        {
            public string Text { get; set; }
        }
    }

    public class AIServiceException : Exception
    {
        public AIServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}