using System.IO;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Progress.Models;
using DeviGames.Atlas.Core.Progress.Services;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Save.Storage;
using NUnit.Framework;

namespace DeviGames.Atlas.Core.Progress.Tests
{
    public class ProgressSaveCoordinatorTests
    {
        private string _testPath;

        [SetUp]
        public void Setup()
        {
            EventBusTestUtility.Reset();

            _testPath = Path.Combine(Path.GetTempPath(), "AtlasProgressSaveTests");

            if (Directory.Exists(_testPath))
                Directory.Delete(_testPath, true);

            Directory.CreateDirectory(_testPath);
        }

        [TearDown]
        public void TearDown()
        {
            EventBusTestUtility.Reset();

            if (Directory.Exists(_testPath))
                Directory.Delete(_testPath, true);
        }

        [Test]
        public async Task MissionCompleted_Should_Update_Progress_And_Save_To_File()
        {
            var progressService = new MissionProgressService();
            progressService.Initialize();

            var saveService = new SaveService(new JsonFileSaveStorage(_testPath));

            var coordinator = new ProgressSaveCoordinator(
                progressService,
                saveService);

            coordinator.Initialize();

            EventBus.Publish(new MissionCompletedEvent("mission_001"));

            await Task.Delay(100);

            bool exists = await saveService.ExistsAsync("missions");
            Assert.IsTrue(exists);

            MissionProgressData loaded =
                await saveService.LoadAsync<MissionProgressData>("missions");

            Assert.IsNotNull(loaded);
            Assert.IsTrue(loaded.IsCompleted("mission_001"));

            coordinator.Shutdown();
            progressService.Shutdown();
        }
    }
}