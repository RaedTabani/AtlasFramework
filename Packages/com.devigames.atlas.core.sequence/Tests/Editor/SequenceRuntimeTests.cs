using System;

using DeviGames.Atlas.Core.Sequence.Models;
using DeviGames.Atlas.Core.Sequence.Services;

using NUnit.Framework;

namespace DeviGames.Atlas.Core.Sequence.Tests
{
    public sealed class SequenceRuntimeTests
    {
        private SequenceDefinition _definition;
        private SequenceRuntime _runtime;

        [SetUp]
        public void SetUp()
        {
            _definition =
                new SequenceDefinition(
                    "sequence.test");

            _runtime =
                new SequenceRuntime(
                    _definition);
        }

        [Test]
        public void Constructor_SetsReadyState()
        {
            Assert.AreEqual(
                SequenceState.Ready,
                _runtime.State);
        }

        [Test]
        public void Constructor_WithNullDefinition_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => new SequenceRuntime(null));
        }

        [Test]
        public void Constructor_WithEmptyId_Throws()
        {
            var definition =
                new SequenceDefinition(
                    string.Empty);

            Assert.Throws<ArgumentException>(
                () => new SequenceRuntime(
                    definition));
        }

        [Test]
        public void Start_FromReady_TransitionsToPlaying()
        {
            bool result =
                _runtime.Start();

            Assert.IsTrue(
                result);

            Assert.AreEqual(
                SequenceState.Playing,
                _runtime.State);
        }

        [Test]
        public void Start_WhenAlreadyPlaying_ReturnsFalse()
        {
            _runtime.Start();

            bool result =
                _runtime.Start();

            Assert.IsFalse(
                result);

            Assert.AreEqual(
                SequenceState.Playing,
                _runtime.State);
        }

        [Test]
        public void Complete_FromPlaying_TransitionsToCompleted()
        {
            _runtime.Start();

            bool result =
                _runtime.Complete();

            Assert.IsTrue(
                result);

            Assert.AreEqual(
                SequenceState.Completed,
                _runtime.State);
        }

        [Test]
        public void Complete_FromReady_ReturnsFalse()
        {
            bool result =
                _runtime.Complete();

            Assert.IsFalse(
                result);

            Assert.AreEqual(
                SequenceState.Ready,
                _runtime.State);
        }

        [Test]
        public void Reset_FromCompleted_ReturnsToReady()
        {
            _runtime.Start();
            _runtime.Complete();

            _runtime.Reset();

            Assert.AreEqual(
                SequenceState.Ready,
                _runtime.State);
        }

        [Test]
        public void Reset_AllowsSequenceToPlayAgain()
        {
            _runtime.Start();
            _runtime.Complete();
            _runtime.Reset();

            Assert.IsTrue(
                _runtime.Start());

            Assert.AreEqual(
                SequenceState.Playing,
                _runtime.State);
        }
    }
}