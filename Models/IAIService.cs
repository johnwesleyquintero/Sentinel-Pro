using System.Threading.Tasks;

namespace SentinelPro.Models
{
    /// <summary>
    /// Interface for AI service operations providing code assistance and natural language processing capabilities.
    /// </summary>
    public interface IAIService
    {
        /// <summary>
        /// Gets AI-powered code completion suggestions based on the provided code context.
        /// </summary>
        /// <param name="codeContext">The code context for which completion is requested.</param>
        /// <returns>A string containing the code completion suggestion.</returns>
        Task<string> GetCodeCompletionAsync(string codeContext);

        /// <summary>
        /// Generates a natural language explanation of the provided code.
        /// </summary>
        /// <param name="code">The code to be explained.</param>
        /// <returns>A string containing the explanation of the code.</returns>
        Task<string> GetCodeExplanationAsync(string code);

        /// <summary>
        /// Processes natural language queries and returns AI-generated responses.
        /// </summary>
        /// <param name="query">The natural language query to process.</param>
        /// <returns>A string containing the AI's response to the query.</returns>
        Task<string> GetNaturalLanguageResponseAsync(string query);

        /// <summary>
        /// Analyzes code for potential errors and issues.
        /// </summary>
        /// <param name="code">The code to analyze for errors.</param>
        /// <returns>A string containing the analysis results and detected issues.</returns>
        Task<string> DetectCodeErrorsAsync(string code);
    }
}