using System;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Sequence.Models;
using DeviGames.Atlas.Core.Sequence.Events;
using DeviGames.Atlas.Core.Sequence.Services;

using NUnit.Framework;

namespace DeviGames.Atlas.Core.Sequence.Tests
{
    public sealed class SequencePlayerTests
    {
        private SequencePlayer _player;
        private SequenceRuntime _sequence;

        private int _completedEventCount;
        private string _completedSequenceId;

        [SetUp]
        public void SetUp()
        {
            _player =
                new SequencePlayer();

            _sequence =
                new SequenceRuntime(
                    new SequenceDefinition(
                        "sequence.test"));

            _completedEventCount =
                0;

            _completedSequenceId =
                null;

            EventBus.Subscribe<SequenceCompletedEvent>(
                OnSequenceCompleted);
        }

        [TearDown]
        public void TearDown()
        {
            EventBus.Unsubscribe<SequenceCompletedEvent>(
                OnSequenceCompleted);
        }

        [Test]
        public void NewPlayer_HasNoActiveSequence()
        {
            Assert.IsNull(
                _player.ActiveSequence);

            Assert.IsFalse(
                _player.IsPlaying);
        }

        [Test]
        public void Play_WithValidSequence_StartsSequence()
        {
            bool result =
                _player.Play(
                    _sequence);

            Assert.IsTrue(
                result);

            Assert.AreSame(
                _sequence,
                _player.ActiveSequence);

            Assert.IsTrue(
                _player.IsPlaying);
        }

        [Test]
        public void Play_WithNullSequence_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => _player.Play(
                    null));
        }

        [Test]
        public void Play_WhenAnotherSequenceIsPlaying_ReturnsFalse()
        {
            _player.Play(
                _sequence);

            var secondSequence =
                new SequenceRuntime(
                    new SequenceDefinition(
                        "sequence.second"));

            bool result =
                _player.Play(
                    secondSequence);

            Assert.IsFalse(
                result);

            Assert.AreSame(
                _sequence,
                _player.ActiveSequence);
        }

        [Test]
        public void Play_WithCompletedSequence_ReturnsFalse()
        {
            _sequence.Start();
            _sequence.Complete();

            bool result =
                _player.Play(
                    _sequence);

            Assert.IsFalse(
                result);

            Assert.IsNull(
                _player.ActiveSequence);

            Assert.IsFalse(
                _player.IsPlaying);
        }

        [Test]
        public void Complete_WhenSequenceIsPlaying_CompletesSequence()
        {
            _player.Play(
                _sequence);

            bool result =
                _player.Complete();

            Assert.IsTrue(
                result);

            Assert.IsNull(
                _player.ActiveSequence);

            Assert.IsFalse(
                _player.IsPlaying);
        }

        [Test]
        public void Complete_WithNoActiveSequence_ReturnsFalse()
        {
            bool result =
                _player.Complete();

            Assert.IsFalse(
                result);

            Assert.AreEqual(
                0,
                _completedEventCount);
        }

        [Test]
        public void Complete_PublishesSequenceCompletedEvent()
        {
            _player.Play(
                _sequence);

            _player.Complete();

            Assert.AreEqual(
                1,
                _completedEventCount);

            Assert.AreEqual(
                "sequence.test",
                _completedSequenceId);
        }

        [Test]
        public void Complete_ClearsActiveSequenceBeforePublishingEvent()
        {
            bool wasPlayingDuringEvent =
                true;

            void OnCompleted(
                SequenceCompletedEvent eventData)
            {
                wasPlayingDuringEvent =
                    _player.IsPlaying;
            }

            EventBus.Subscribe<SequenceCompletedEvent>(
                OnCompleted);

            try
            {
                _player.Play(
                    _sequence);

                _player.Complete();

                Assert.IsFalse(
                    wasPlayingDuringEvent);
            }
            finally
            {
                EventBus.Unsubscribe<SequenceCompletedEvent>(
                    OnCompleted);
            }
        }

        [Test]
        public void CompletedSequence_CanBeResetAndPlayedAgain()
        {
            _player.Play(
                _sequence);

            _player.Complete();

            _sequence.Reset();

            bool result =
                _player.Play(
                    _sequence);

            Assert.IsTrue(
                result);

            Assert.AreSame(
                _sequence,
                _player.ActiveSequence);
        }

        private void OnSequenceCompleted(
            SequenceCompletedEvent eventData)
        {
            _completedEventCount++;

            _completedSequenceId =
                eventData.SequenceId;
        }
    }
}