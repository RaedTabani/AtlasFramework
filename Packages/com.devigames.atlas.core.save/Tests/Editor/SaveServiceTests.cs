using System;
using System.IO;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Save.Events;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Save.Storage;
using NUnit.Framework;

namespace DeviGames.Atlas.Core.Save.Tests
{
    [Serializable]
    public class DummyData
    {
        public int Value;
        public string Name;
    }

    /// <summary>
    /// Exercises SaveService directly against a real JsonFileSaveStorage backed
    /// by a temp folder. No storage test double is used - the four Save/Load
    /// events published by SaveService are the source of truth for what happened,
    /// which also doubles as a black-box check that the events actually fire.
    /// </summary>
    public class SaveServiceTests
    {
        private string _rootPath;
        private JsonFileSaveStorage _storage;
        private SaveService _service;

        // Counts driven entirely by subscribing to SaveService's events.
        private int _saveCompletedCount;
        private int _saveFailedCount;
        private int _loadCompletedCount;
        private int _loadFailedCount;

        // Last-seen event payloads, for assertions that need more than a count.
        // Nullable, since the events themselves are readonly structs and can't be null.
        private SaveFailedEvent? _lastSaveFailed;
        private LoadFailedEvent? _lastLoadFailed;
        private SaveCompletedEvent? _lastSaveCompleted;
        private LoadCompletedEvent? _lastLoadCompleted;

        [SetUp]
        public void Setup()
        {
            _rootPath = Path.Combine(Path.GetTempPath(), "AtlasSaveServiceTests_" + Guid.NewGuid());
            _storage = new JsonFileSaveStorage(_rootPath);
            _service = new SaveService(_storage);

            _saveCompletedCount = 0;
            _saveFailedCount = 0;
            _loadCompletedCount = 0;
            _loadFailedCount = 0;
            _lastSaveFailed = null;
            _lastLoadFailed = null;
            _lastSaveCompleted = null;
            _lastLoadCompleted = null;

            EventBusTestUtility.Reset();
            EventBus.Subscribe<SaveCompletedEvent>(OnSaveCompleted);
            EventBus.Subscribe<SaveFailedEvent>(OnSaveFailed);
            EventBus.Subscribe<LoadCompletedEvent>(OnLoadCompleted);
            EventBus.Subscribe<LoadFailedEvent>(OnLoadFailed);
        }

        [TearDown]
        public void TearDown()
        {
            EventBus.Unsubscribe<SaveCompletedEvent>(OnSaveCompleted);
            EventBus.Unsubscribe<SaveFailedEvent>(OnSaveFailed);
            EventBus.Unsubscribe<LoadCompletedEvent>(OnLoadCompleted);
            EventBus.Unsubscribe<LoadFailedEvent>(OnLoadFailed);

            if (Directory.Exists(_rootPath))
                Directory.Delete(_rootPath, recursive: true);
        }

        private void OnSaveCompleted(SaveCompletedEvent e)
        {
            _saveCompletedCount++;
            _lastSaveCompleted = e;
        }

        private void OnSaveFailed(SaveFailedEvent e)
        {
            _saveFailedCount++;
            _lastSaveFailed = e;
        }

        private void OnLoadCompleted(LoadCompletedEvent e)
        {
            _loadCompletedCount++;
            _lastLoadCompleted = e;
        }

        private void OnLoadFailed(LoadFailedEvent e)
        {
            _loadFailedCount++;
            _lastLoadFailed = e;
        }

        // ---------- Construction ----------

        [Test]
        public void Constructor_Should_Throw_When_Storage_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new SaveService(null));
        }

        // ---------- Key validation (no events should fire) ----------

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void SaveAsync_Should_Throw_On_Invalid_Key_And_Publish_Nothing(string key)
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.SaveAsync(key, new DummyData()));

            Assert.AreEqual(0, _saveCompletedCount);
            Assert.AreEqual(0, _saveFailedCount);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void LoadAsync_Should_Throw_On_Invalid_Key_And_Publish_Nothing(string key)
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.LoadAsync<DummyData>(key));

            Assert.AreEqual(0, _loadCompletedCount);
            Assert.AreEqual(0, _loadFailedCount);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void ExistsAsync_Should_Throw_On_Invalid_Key(string key)
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.ExistsAsync(key));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void DeleteAsync_Should_Throw_On_Invalid_Key(string key)
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await _service.DeleteAsync(key));
        }

        // ---------- Success paths, verified purely through event counts ----------

        [Test]
        public async Task SaveAsync_Should_Increment_SaveCompletedCount_On_Success()
        {
            await _service.SaveAsync("slot1", new DummyData { Value = 1, Name = "A" });

            Assert.AreEqual(1, _saveCompletedCount);
            Assert.AreEqual(0, _saveFailedCount);
            Assert.IsTrue(_lastSaveCompleted.HasValue);
            Assert.AreEqual("slot1", _lastSaveCompleted.Value.Key);
        }

        [Test]
        public async Task SaveAsync_Called_Multiple_Times_Should_Increment_SaveCompletedCount_Each_Time()
        {
            await _service.SaveAsync("slot1", new DummyData());
            await _service.SaveAsync("slot2", new DummyData());
            await _service.SaveAsync("slot1", new DummyData());

            Assert.AreEqual(3, _saveCompletedCount);
            Assert.AreEqual(0, _saveFailedCount);
        }

        [Test]
        public async Task LoadAsync_Should_Increment_LoadCompletedCount_On_Success()
        {
            await _service.SaveAsync("slot1", new DummyData { Value = 5, Name = "B" });

            DummyData result = await _service.LoadAsync<DummyData>("slot1");

            Assert.AreEqual(1, _loadCompletedCount);
            Assert.AreEqual(0, _loadFailedCount);
            Assert.IsTrue(_lastLoadCompleted.HasValue);
            Assert.AreEqual("slot1", _lastLoadCompleted.Value.Key);
            Assert.AreEqual(5, result.Value);
        }

        [Test]
        public async Task LoadAsync_For_Missing_Key_Should_Still_Increment_LoadCompletedCount()
        {
            // LoadAsync returns default(T) for a missing key rather than throwing,
            // so this is a "completed" load, not a "failed" one.
            DummyData result = await _service.LoadAsync<DummyData>("nonexistent");

            Assert.AreEqual(1, _loadCompletedCount);
            Assert.AreEqual(0, _loadFailedCount);
            Assert.IsNull(result);
        }

        [Test]
        public async Task ExistsAsync_Should_Return_True_Only_After_Save()
        {
            Assert.IsFalse(await _service.ExistsAsync("slot1"));

            await _service.SaveAsync("slot1", new DummyData());

            Assert.IsTrue(await _service.ExistsAsync("slot1"));
        }

        [Test]
        public async Task DeleteAsync_Should_Remove_The_Saved_Entry()
        {
            await _service.SaveAsync("slot1", new DummyData());
            Assert.IsTrue(await _service.ExistsAsync("slot1"));

            await _service.DeleteAsync("slot1");

            Assert.IsFalse(await _service.ExistsAsync("slot1"));
        }

        // ---------- Failure paths, verified purely through event counts ----------

        [Test]
        public void SaveAsync_Should_Increment_SaveFailedCount_And_Rethrow_When_Write_Target_Is_Invalid()
        {
            // Force a real storage-layer failure: pre-create a directory at the
            // exact path JsonFileSaveStorage will try to write a file to, so
            // File.WriteAllTextAsync throws (reliable on both Windows and Unix).
            string key = "slot1";
            string collidingPath = Path.Combine(_rootPath, $"{key}.json");
            Directory.CreateDirectory(collidingPath);

            Assert.ThrowsAsync<IOException>(async () => await _service.SaveAsync(key, new DummyData()));

            Assert.AreEqual(1, _saveFailedCount);
            Assert.AreEqual(0, _saveCompletedCount);
            Assert.IsTrue(_lastSaveFailed.HasValue);
            Assert.AreEqual(key, _lastSaveFailed.Value.Key);
        }


        [Test]
        public async Task Failed_Save_Should_Not_Also_Increment_SaveCompletedCount()
        {
            string key = "slot1";
            Directory.CreateDirectory(Path.Combine(_rootPath, $"{key}.json"));

            try
            {
                await _service.SaveAsync(key, new DummyData());
            }
            catch (IOException)
            {
                // expected - asserting on counts below
            }

            Assert.AreEqual(0, _saveCompletedCount);
            Assert.AreEqual(1, _saveFailedCount);
        }
    }
}