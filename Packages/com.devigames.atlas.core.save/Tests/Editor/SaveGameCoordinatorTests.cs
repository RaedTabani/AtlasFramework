using System;
using System.Threading.Tasks;

using NUnit.Framework;

using DeviGames.Atlas.Core.Save.Collections;
using DeviGames.Atlas.Core.Save.Interfaces;
using DeviGames.Atlas.Core.Save.Services;

namespace DeviGames.Atlas.Core.Save.Tests
{
    public sealed class SaveGameCoordinatorTests
    {
        private SaveParticipantCollection _participants;
        private SaveGameCoordinator _coordinator;

        [SetUp]
        public void SetUp()
        {
            _participants =
                new SaveParticipantCollection();

            _coordinator =
                new SaveGameCoordinator(
                    _participants);
        }

        [Test]
        public async Task SaveAsync_SavesAllParticipants()
        {
            var first =
                new TestSaveParticipant(
                    "first");

            var second =
                new TestSaveParticipant(
                    "second");

            _participants.Add(
                first);

            _participants.Add(
                second);

            await _coordinator.SaveAsync();

            Assert.That(
                first.SaveCount,
                Is.EqualTo(1));

            Assert.That(
                second.SaveCount,
                Is.EqualTo(1));
        }

        [Test]
        public async Task LoadAsync_LoadsAllParticipants()
        {
            var first =
                new TestSaveParticipant(
                    "first");

            var second =
                new TestSaveParticipant(
                    "second");

            _participants.Add(
                first);

            _participants.Add(
                second);

            await _coordinator.LoadAsync();

            Assert.That(
                first.LoadCount,
                Is.EqualTo(1));

            Assert.That(
                second.LoadCount,
                Is.EqualTo(1));
        }

        [Test]
        public void Add_DuplicateKey_Throws()
        {
            _participants.Add(
                new TestSaveParticipant(
                    "unlocks"));

            Assert.Throws<InvalidOperationException>(
                () =>
                    _participants.Add(
                        new TestSaveParticipant(
                            "unlocks")));
        }

        [Test]
        public void Add_NullParticipant_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                    _participants.Add(
                        null));
        }

        [Test]
        public void Add_EmptyKey_Throws()
        {
            Assert.Throws<ArgumentException>(
                () =>
                    _participants.Add(
                        new TestSaveParticipant(
                            "")));
        }

        [Test]
        public async Task SaveAsync_NoParticipants_CompletesSuccessfully()
        {
            Assert.DoesNotThrowAsync(
                async () =>
                    await _coordinator.SaveAsync());
        }

        [Test]
        public async Task LoadAsync_NoParticipants_CompletesSuccessfully()
        {
            Assert.DoesNotThrowAsync(
                async () =>
                    await _coordinator.LoadAsync());
        }

        private sealed class TestSaveParticipant :
            ISaveParticipant
        {
            public string Key { get; }

            public int SaveCount { get; private set; }

            public int LoadCount { get; private set; }

            public TestSaveParticipant(
                string key)
            {
                Key =
                    key;
            }

            public Task SaveAsync()
            {
                SaveCount++;

                return Task.CompletedTask;
            }

            public Task LoadAsync()
            {
                LoadCount++;

                return Task.CompletedTask;
            }
        }
    }
}