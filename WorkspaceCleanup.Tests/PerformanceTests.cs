using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;

namespace WorkspaceCleanup.Tests
{
    [TestFixture]
    [Category("Performance")]
    public class BackupPerformanceTests
    {
        private const string TestDataPath = @"TestData/BaseDataset";

        [Test]
        public async Task CreateBackup_10GBDataset_Under60Seconds()
        {
            var testData = Path.Combine(TestContext.CurrentContext.TestDirectory, "10GB_TestData");
            await PerformanceHelper.MeasureAsync(async () => 
            {
                await BackupService.CreateBackupAsync(testData);
            }, maxDuration: 60000);
        }

        [Test]
        public async Task RestoreBackup_10GBDataset_Under45Seconds()
        {
            var backup = await BackupService.CreateBackupAsync(TestDataPath);
            await PerformanceHelper.MeasureAsync(async () => 
            {
                await BackupService.RestoreBackupAsync(backup.Id);
            }, maxDuration: 45000);
        }

        [Test]
        public async Task ConcurrentBackups_5Parallel_Under90Seconds()
        {
            var tasks = new List<Task>();
            await PerformanceHelper.MeasureAsync(async () =>
            {
                for (int i = 0; i < 5; i++)
                {
                    tasks.Add(BackupService.CreateBackupAsync(TestDataPath));
                }
                await Task.WhenAll(tasks);
            }, maxDuration: 90000);
        }

        [Test]
        public async Task LargeDataset_100GB_CompressionUnder75Seconds()
        {
            var largeData = Path.Combine(TestContext.CurrentContext.TestDirectory, "100GB_StressData");
            await PerformanceHelper.MeasureAsync(async () =>
            {
                await BackupService.CreateCompressedBackupAsync(largeData);
            }, maxDuration: 75000);
        }

        [Test]
        public void MemoryUsage_UnderLoad_StableFootprint()
        {
            var monitor = new PerformanceMonitor();
            monitor.Start();

            Parallel.For(0, 10, i => {
                BackupService.ProcessLargeDataset();
                BackupService.CleanTemporaryFiles();
            });

            monitor.Stop();
            Assert.Less(monitor.PeakMemoryMB, 800, "Memory footprint exceeded under load");
            Assert.Less(monitor.AverageMemoryMB, 500, "Average memory exceeded under sustained load");
        }

        [Test]
        public async Task BackupRestoreCycle_DataIntegrityVerification()
        {
            var backup = await BackupService.CreateBackupAsync(TestDataPath);
            var restorePath = Path.Combine(Path.GetTempPath(), "RestoreVerification");

            await PerformanceHelper.MeasureAsync(async () =>
            {
                await BackupService.RestoreBackupAsync(backup.Id, restorePath);
            }, maxDuration: 60000);

            Assert.That(DirectoryUtils.DirectoryCompare(TestDataPath, restorePath),
                "Original and restored datasets differ");
        }
    }
}