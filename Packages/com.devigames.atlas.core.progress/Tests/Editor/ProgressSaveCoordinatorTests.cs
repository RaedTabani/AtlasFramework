using System.IO;
using System.Threading.Tasks;
using System.Linq;
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

    }
}