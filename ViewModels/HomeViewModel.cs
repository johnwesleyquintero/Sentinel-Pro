using System.Windows.Input;
using System.Windows;
using SentinelPro.Models;
using SentinelPro.Services;

namespace SentinelPro.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly IAIService _aiService;
        private string _aiResponse = string.Empty;
        private bool _isProcessing;

        public string AIResponse
        {
            get => _aiResponse;
            private set => SetProperty(ref _aiResponse, value);
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            private set => SetProperty(ref _isProcessing, value);
        }

        public ICommand AskAICommand { get; }

        public HomeViewModel(IAIService aiService)
        {
            _aiService = aiService;
            AskAICommand = new RelayCommand(async param => await AskAI(param as string),
                param => !IsProcessing && !string.IsNullOrWhiteSpace(param as string));
        }

        private async Task AskAI(string? query)
        {
            if (string.IsNullOrWhiteSpace(query)) return;

            try
            {
                IsProcessing = true;
                AIResponse = await _aiService.GetNaturalLanguageResponseAsync(query); // Corrected method call
            }
            catch (AIServiceException aiEx) // Catch specific exception if available
            {
                AIResponse = $"AI Service Error: {aiEx.Message}";
                // Optionally log the inner exception: _logger.LogError(aiEx.InnerException, "AI Service failed");
            }
            catch (Exception ex) // Catch general exceptions
            {
                AIResponse = $"Error: {ex.Message}";
            }
            finally
            {
                IsProcessing = false;
            }
        }
    }
}
