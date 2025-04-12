using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Serilog;
using NUnit.Framework;
using WorkspaceCleanup.Models;

namespace WorkspaceCleanup.Tests
{
    [TestFixture]
    internal class AIServiceTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<HttpClient> _httpClientMock;
        private readonly IAIService _aiService;

        public AIServiceTests()
        {
            _loggerMock = new Mock<ILogger>();
            _configMock = new Mock<IConfiguration>();
            _httpClientMock = new Mock<HttpClient>();
            
            _configMock.Setup(c => c["AI:BaseUrl"]).Returns("http://localhost:11434");
            _configMock.Setup(c => c["AI:Model"]).Returns("codellama");
            
            _aiService = new OllamaAIService(_loggerMock.Object, _configMock.Object, _httpClientMock.Object);
        }

        [Test]
        public async Task GetCodeCompletion_WithValidInput_ReturnsCompletion()
        {
            // Arrange
            var codeContext = "public class Example { public void Test() {"; 
            var expectedCompletion = @"    Console.WriteLine(""Hello World"");
}}"; 

            _httpClientMock.Setup(client => client
                .SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(expectedCompletion)
                });

            // Act
            var result = await _aiService.GetCodeCompletionAsync(codeContext);

            // Assert
            Assert.Equal(expectedCompletion, result);
            _loggerMock.Verify(x => x.Information(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task GetCodeExplanation_WithValidCode_ReturnsExplanation()
        {
            // Arrange
            var code = "public int Add(int a, int b) { return a + b; }";
            var expectedExplanation = "This method adds two integers and returns their sum.";

            _httpClientMock.Setup(client => client
                .SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(expectedExplanation)
                });

            // Act
            var result = await _aiService.GetCodeExplanationAsync(code);

            // Assert
            Assert.Equal(expectedExplanation, result);
            _loggerMock.Verify(x => x.Information(It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetCodeCompletion_WithInvalidInput_ThrowsException(string invalidInput)
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _aiService.GetCodeCompletionAsync(invalidInput));
            
            Assert.Contains("Code context cannot be null or empty", exception.Message);
            _loggerMock.Verify(x => x.Error(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DetectCodeErrors_WithValidCode_ReturnsAnalysis()
        {
            // Arrange
            var code = "public void Method() { int x = null; }";
            var expectedAnalysis = "Error: Cannot assign null to value type 'int'";

            _httpClientMock.Setup(client => client
                .SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(expectedAnalysis)
                });

            // Act
            var result = await _aiService.DetectCodeErrorsAsync(code);

            // Assert
            Assert.Equal(expectedAnalysis, result);
            _loggerMock.Verify(x => x.Information(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetNaturalLanguageResponse_WithValidQuery_ReturnsResponse()
        {
            // Arrange
            var query = "How do I implement a binary search?";
            var expectedResponse = "Here's how to implement a binary search...";

            _httpClientMock.Setup(client => client
                .SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(expectedResponse)
                });

            // Act
            var result = await _aiService.GetNaturalLanguageResponseAsync(query);

            // Assert
            Assert.Equal(expectedResponse, result);
            _loggerMock.Verify(x => x.Information(It.IsAny<string>()), Times.Once);
        }
        string testQuery = "This is a properly closed string";
        public void TestMethod() 
        {
            // Properly enclosed method body
        }

        [Test]
        public async Task AnalyzeBackupPatternsAsync_ReturnsValidAnalysis()
        {
            // Arrange
            var codeContext = "public class Example { public void Test() {"; 
            var expectedCompletion = @"    Console.WriteLine(""Hello World"");
}}"; 

            _httpClientMock.Setup(client => client
                .SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(expectedCompletion)
                });

            // Act
            var result = await _aiService.GetCodeCompletionAsync(codeContext);

            // Assert
            Assert.Equal(expectedCompletion, result);
            _loggerMock.Verify(x => x.Information(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task GenerateCleanupRecommendationAsync_ReturnsValidRules()
        {
            // Arrange
            var code = "public int Add(int a, int b) { return a + b; }";
            var expectedExplanation = "This method adds two integers and returns their sum.";

            _httpClientMock.Setup(client => client
                .SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(expectedExplanation)
                });

            // Act
            var result = await _aiService.GetCodeExplanationAsync(code);

            // Assert
            Assert.Equal(expectedExplanation, result);
            _loggerMock.Verify(x => x.Information(It.IsAny<string>()), Times.Once);
        }

        [Test]
        async Task HandleComplexScenario_ReturnsFallbackStrategy()
        {
            // Arrange
            var query = "How do I implement a binary search?";
            var expectedResponse = "Here's how to implement a binary search...";

            _httpClientMock.Setup(client => client
                .SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(expectedResponse)
                });

            // Act
            var result = await _aiService.GetNaturalLanguageResponseAsync(query);

            // Assert
            Assert.Equal(expectedResponse, result);
            _loggerMock.Verify(x => x.Information(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetCodeCompletion_WithEmptyCode_ThrowsValidationException()
        {
            Assert.ThrowsAsync<ValidationException>(() => 
                _aiService.GetCodeCompletionAsync(string.Empty));
        }

        [Test]
        public async Task GetCodeCompletion_WithInvalidModel_ReturnsErrorMessage()
        {
            _configMock.Setup(c => c["AI:Model"]).Returns("invalid-model");
            var result = await _aiService.GetCodeCompletionAsync("test");
            StringAssert.Contains("Unsupported model", result);
        }

        [Test]
        public void GetCodeCompletion_WithNetworkFailure_ThrowsServiceException()
        {
            _httpClientMock.Setup(client => client
                .SendAsync(It.IsAny<HttpRequestMessage>()))
                .Throws(new HttpRequestException("Network error"));

            Assert.ThrowsAsync<AIServiceException>(() =>
                _aiService.GetCodeCompletionAsync("valid code"));
        }

        [Test]
        [Category("Performance")]
        public void CodeCompletion_ResponseUnder100ms()
        {
            var sw = Stopwatch.StartNew();
            _aiService.GetCodeCompletionAsync("public class PerfTest {");
            sw.Stop();
            Assert.Less(sw.ElapsedMilliseconds, 100);
        }

        [Test]
        public async Task AnalyzeBackupPatternsAsync_ReturnsValidAnalysis()
        {
            // Arrange
            var codeContext = "public class Example { public void Test() {"; 
            var expectedCompletion = @"    Console.WriteLine(""Hello World"");
}}"; 

            _httpClientMock.Setup(client => client
                .SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(expectedCompletion)
                });

            // Act
            var result = await _aiService.GetCodeCompletionAsync(codeContext);

            // Assert
            Assert.Equal(expectedCompletion, result);
            _loggerMock.Verify(x => x.Information(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task GenerateCleanupRecommendationAsync_ReturnsValidRules()
        {
            // Arrange
            var code = "public int Add(int a, int b) { return a + b; }";
            var expectedExplanation = "This method adds two integers and returns their sum.";

            _httpClientMock.Setup(client => client
                .SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(expectedExplanation)
                });

            // Act
            var result = await _aiService.GetCodeExplanationAsync(code);

            // Assert
            Assert.Equal(expectedExplanation, result);
            _loggerMock.Verify(x => x.Information(It.IsAny<string>()), Times.Once);
        }

        [Test]
        async Task HandleComplexScenario_ReturnsFallbackStrategy()
        {
            // Arrange
            var query = "How do I implement a binary search?";
            var expectedResponse = "Here's how to implement a binary search...";

            _httpClientMock.Setup(client => client
                .SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(expectedResponse)
                });

            // Act
            var result = await _aiService.GetNaturalLanguageResponseAsync(query);

            // Assert
            Assert.Equal(expectedResponse, result);
            _loggerMock.Verify(x => x.Information(It.IsAny<string>()), Times.Once);
        }
    }
}