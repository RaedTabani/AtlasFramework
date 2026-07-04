using System.IO;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Save.Storage;
using NUnit.Framework;

namespace DeviGames.Atlas.Core.Save.Tests
{
    public class SaveServiceTests
    {
        private string _testPath;
        private SaveService _saveService;

        [SetUp]
        public void Setup()
        {
            _testPath = Path.Combine(Path.GetTempPath(), "AtlasSaveTests");

            if (Directory.Exists(_testPath))
                Directory.Delete(_testPath, true);

            Directory.CreateDirectory(_testPath);

            _saveService = new SaveService(new JsonFileSaveStorage(_testPath));
        }

        [TearDown]
        public void Teardown()
        {
            if (Directory.Exists(_testPath))
                Directory.Delete(_testPath, true);
        }

        [Test]
        public async Task SaveAsync_ThenLoadAsync_Should_Return_Saved_Data()
        {
            var data = new TestSaveData
            {
                PlayerName = "Tester",
                Coins = 100
            };

            await _saveService.SaveAsync("profile", data);

            TestSaveData loaded = await _saveService.LoadAsync<TestSaveData>("profile");

            Assert.IsNotNull(loaded);
            Assert.AreEqual("Tester", loaded.PlayerName);
            Assert.AreEqual(100, loaded.Coins);
        }

        [Test]
        public async Task ExistsAsync_Should_Return_True_After_Save()
        {
            await _saveService.SaveAsync("profile", new TestSaveData());

            bool exists = await _saveService.ExistsAsync("profile");

            Assert.IsTrue(exists);
        }

        [Test]
        public async Task DeleteAsync_Should_Remove_Save()
        {
            await _saveService.SaveAsync("profile", new TestSaveData());

            await _saveService.DeleteAsync("profile");

            bool exists = await _saveService.ExistsAsync("profile");

            Assert.IsFalse(exists);
        }

        [Test]
        public async Task LoadAsync_When_File_Missing_Should_Return_Default()
        {
            TestSaveData loaded = await _saveService.LoadAsync<TestSaveData>("missing");

            Assert.IsNull(loaded);
        }

        [Test]
        public void SaveAsync_With_Invalid_Key_Should_Throw()
        {
            Assert.ThrowsAsync<System.ArgumentException>(async () =>
            {
                await _saveService.SaveAsync("", new TestSaveData());
            });
        }
    }
}