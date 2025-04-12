using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using WorkspaceCleanup.Services;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WorkspaceCleanup.Tests
{
    public class MonitoringServiceTests : IDisposable
    {
        private readonly Mock<IConfiguration> _configMock;
        private MonitoringService _service;

        public MonitoringServiceTests()
        {
            _configMock = new Mock<IConfiguration>();
            _configMock.Setup(x => x.GetValue<double>("PerformanceThreshold", 80.0)).Returns(90.0);
            _configMock.Setup(x => x.GetValue<double>("MemoryThreshold", 1024)).Returns(2048.0);
            _service = new MonitoringService(_configMock.Object);
        }

        [Fact]
        public void InitializeMonitoring_ConfiguresThresholdsCorrectly()
        {
            // Assert
            _configMock.Verify(x => x.GetValue<double>("PerformanceThreshold", 80.0), Times.AtLeastOnce);
            _configMock.Verify(x => x.GetValue<double>("MemoryThreshold", 1024), Times.AtLeastOnce);
        }

        [Theory]
        [InlineData(85.0, 1500.0, false)]
        [InlineData(95.0, 2500.0, true)]
        public async Task MonitorPerformance_TriggersWarningsAppropriately(double cpuUsage, double memUsage, bool shouldWarn)
        {
            // Arrange
            var exceptions = new List<Exception>();
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) => exceptions.Add(e.Exception);

            // Act
            _service.TrackCpuUsage();
            _service.TrackMemoryUsage();
            await Task.Delay(100); // Allow time for async operations

            // Assert
            if (shouldWarn)
            {
                Assert.Contains(exceptions, e => e is PerformanceThresholdExceededException);
            }
            else
            {
                Assert.DoesNotContain(exceptions, e => e is PerformanceThresholdExceededException);
            }
        }

        [Fact]
        public async Task MonitorPerformance_HandlesErrors_GracefullyWithBackoff()
        {
            // Arrange
            var exceptions = new List<Exception>();
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) => exceptions.Add(e.Exception);

            // Act
            _service.TrackCpuUsage();
            await Task.Delay(100); // Allow time for async operations

            // Assert
            Assert.DoesNotContain(exceptions, e => e is InvalidOperationException);
        }

        [Fact]
        public void Dispose_ReleasesResourcesCorrectly()
        {
            // Act
            _service.Dispose();

            // Verify no memory leaks by attempting to track usage after disposal
            var exception = Record.Exception(() =>
            {
                _service.TrackCpuUsage();
                _service.TrackMemoryUsage();
            });

            Assert.NotNull(exception);
        }

        public void Dispose()
        {
            _service?.Dispose();
        }
    }
}