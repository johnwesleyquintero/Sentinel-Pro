namespace SentinelPro.Services.Interfaces
{
    public interface IErrorHandlingService
    {
        void HandleException(System.Exception ex);
    }
}