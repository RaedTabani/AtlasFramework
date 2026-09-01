using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Sequence.Models;
using DeviGames.Atlas.Core.Sequence.Events;
using DeviGames.Atlas.Core.Sequence.Interfaces;
using DeviGames.Atlas.Core.Sequence.Services;
using DeviGames.Atlas.Core.Sequence.Steps;

using NUnit.Framework;

namespace DeviGames.Atlas.Core.Sequence.Tests
{
    public sealed class SequencePlayerTests
    {
        private SequencePlayer _player;

        private int _completedEventCount;
        private string _completedSequenceId;

        [SetUp]
        public void SetUp()
        {
            _player =
                new SequencePlayer();

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
        public void Play_WithNullSequence_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => _player.Play(
                    null));
        }

        [Test]
        public void Play_EntersFirstStep()
        {
            var step =
                new TestSequenceStep(
                    completeOnEnter: false);

            SequenceRuntime sequence =
                CreateSequence(
                    step);

            bool result =
                _player.Play(
                    sequence);

            Assert.IsTrue(
                result);

            Assert.AreEqual(
                1,
                step.EnterCount);

            Assert.AreSame(
                step,
                sequence.CurrentStep);

            Assert.IsTrue(
                _player.IsPlaying);
        }

        [Test]
        public void Play_AutoAdvancesCompletedSteps()
        {
            var firstStep =
                new TestSequenceStep(
                    completeOnEnter: true);

            var secondStep =
                new TestSequenceStep(
                    completeOnEnter: true);

            var blockingStep =
                new TestSequenceStep(
                    completeOnEnter: false);

            SequenceRuntime sequence =
                CreateSequence(
                    firstStep,
                    secondStep,
                    blockingStep);

            _player.Play(
                sequence);

            Assert.AreEqual(
                1,
                firstStep.EnterCount);

            Assert.AreEqual(
                1,
                secondStep.EnterCount);

            Assert.AreEqual(
                1,
                blockingStep.EnterCount);

            Assert.AreSame(
                blockingStep,
                sequence.CurrentStep);

            Assert.IsTrue(
                _player.IsPlaying);
        }

        [Test]
        public void Play_StopsAtIncompleteStep()
        {
            var blockingStep =
                new TestSequenceStep(
                    completeOnEnter: false);

            var nextStep =
                new TestSequenceStep(
                    completeOnEnter: true);

            SequenceRuntime sequence =
                CreateSequence(
                    blockingStep,
                    nextStep);

            _player.Play(
                sequence);

            Assert.AreEqual(
                1,
                blockingStep.EnterCount);

            Assert.AreEqual(
                0,
                nextStep.EnterCount);

            Assert.AreSame(
                blockingStep,
                sequence.CurrentStep);
        }

        [Test]
        public void Advance_AfterBlockingStepCompletes_ContinuesProcessing()
        {
            var blockingStep =
                new TestSequenceStep(
                    completeOnEnter: false);

            var nextStep =
                new TestSequenceStep(
                    completeOnEnter: false);

            SequenceRuntime sequence =
                CreateSequence(
                    blockingStep,
                    nextStep);

            _player.Play(
                sequence);

            blockingStep.Complete();

            bool result =
                _player.Advance();

            Assert.IsTrue(
                result);

            Assert.AreEqual(
                1,
                nextStep.EnterCount);

            Assert.AreSame(
                nextStep,
                sequence.CurrentStep);
        }

        [Test]
        public void Advance_WhileCurrentStepIsStillIncomplete_DoesNotEnterItAgain()
        {
            var blockingStep =
                new TestSequenceStep(
                    completeOnEnter: false);

            SequenceRuntime sequence =
                CreateSequence(
                    blockingStep);

            _player.Play(
                sequence);

            bool result =
                _player.Advance();

            Assert.IsTrue(
                result);

            Assert.AreEqual(
                1,
                blockingStep.EnterCount);

            Assert.AreSame(
                blockingStep,
                sequence.CurrentStep);
        }

        [Test]
        public void Advance_WithNoActiveSequence_ReturnsFalse()
        {
            bool result =
                _player.Advance();

            Assert.IsFalse(
                result);
        }

        [Test]
        public void Continue_WithNoActiveSequence_ReturnsFalse()
        {
            bool result =
                _player.Continue();

            Assert.IsFalse(
                result);
        }

        [Test]
        public void Continue_WhenCurrentStepIsNotContinuable_ReturnsFalse()
        {
            var blockingStep =
                new TestSequenceStep(
                    completeOnEnter: false);

            SequenceRuntime sequence =
                CreateSequence(
                    blockingStep);

            _player.Play(
                sequence);

            bool result =
                _player.Continue();

            Assert.IsFalse(
                result);

            Assert.AreSame(
                blockingStep,
                sequence.CurrentStep);
        }

        [Test]
        public void Continue_WithWaitForContinueStep_AdvancesToNextStep()
        {
            var waitStep =
                new WaitForContinueStep();

            var nextStep =
                new TestSequenceStep(
                    completeOnEnter: false);

            SequenceRuntime sequence =
                CreateSequence(
                    waitStep,
                    nextStep);

            _player.Play(
                sequence);

            bool result =
                _player.Continue();

            Assert.IsTrue(
                result);

            Assert.AreSame(
                nextStep,
                sequence.CurrentStep);

            Assert.AreEqual(
                1,
                nextStep.EnterCount);
        }

        [Test]
        public void Continue_OnFinalWaitForContinueStep_CompletesSequence()
        {
            var waitStep =
                new WaitForContinueStep();

            SequenceRuntime sequence =
                CreateSequence(
                    waitStep);

            _player.Play(
                sequence);

            bool result =
                _player.Continue();

            Assert.IsTrue(
                result);

            Assert.IsFalse(
                _player.IsPlaying);

            Assert.IsNull(
                _player.ActiveSequence);

            Assert.AreEqual(
                1,
                _completedEventCount);

            Assert.AreEqual(
                "sequence.test",
                _completedSequenceId);
        }

        [Test]
        public void Continue_CannotCompleteSameWaitStepTwice()
        {
            var waitStep =
                new WaitForContinueStep();

            var nextStep =
                new TestSequenceStep(
                    completeOnEnter: false);

            SequenceRuntime sequence =
                CreateSequence(
                    waitStep,
                    nextStep);

            _player.Play(
                sequence);

            bool firstResult =
                _player.Continue();

            bool secondResult =
                _player.Continue();

            Assert.IsTrue(
                firstResult);

            Assert.IsFalse(
                secondResult);

            Assert.AreSame(
                nextStep,
                sequence.CurrentStep);
        }

        [Test]
        public void Play_WhenAnotherSequenceIsPlaying_ReturnsFalse()
        {
            SequenceRuntime firstSequence =
                CreateSequence(
                    new TestSequenceStep(
                        completeOnEnter: false));

            SequenceRuntime secondSequence =
                CreateSequence(
                    "sequence.second",
                    new TestSequenceStep(
                        completeOnEnter: false));

            _player.Play(
                firstSequence);

            bool result =
                _player.Play(
                    secondSequence);

            Assert.IsFalse(
                result);

            Assert.AreSame(
                firstSequence,
                _player.ActiveSequence);
        }

        [Test]
        public void Play_WithCompletedSequence_ReturnsFalse()
        {
            SequenceRuntime sequence =
                CreateSequence();

            sequence.Start();
            sequence.Complete();

            bool result =
                _player.Play(
                    sequence);

            Assert.IsFalse(
                result);

            Assert.IsFalse(
                _player.IsPlaying);
        }

        [Test]
        public void Play_WithOnlyImmediateSteps_CompletesSequence()
        {
            SequenceRuntime sequence =
                CreateSequence(
                    new TestSequenceStep(
                        completeOnEnter: true),
                    new TestSequenceStep(
                        completeOnEnter: true));

            bool result =
                _player.Play(
                    sequence);

            Assert.IsTrue(
                result);

            Assert.IsFalse(
                _player.IsPlaying);

            Assert.IsNull(
                _player.ActiveSequence);
        }

        [Test]
        public void Play_WithEmptySequence_CompletesImmediately()
        {
            SequenceRuntime sequence =
                CreateSequence();

            bool result =
                _player.Play(
                    sequence);

            Assert.IsTrue(
                result);

            Assert.IsFalse(
                _player.IsPlaying);

            Assert.AreEqual(
                1,
                _completedEventCount);
        }

        [Test]
        public void CompletingFinalBlockingStep_CompletesSequenceOnAdvance()
        {
            var blockingStep =
                new TestSequenceStep(
                    completeOnEnter: false);

            SequenceRuntime sequence =
                CreateSequence(
                    blockingStep);

            _player.Play(
                sequence);

            blockingStep.Complete();

            _player.Advance();

            Assert.IsFalse(
                _player.IsPlaying);

            Assert.IsNull(
                _player.ActiveSequence);

            Assert.AreEqual(
                1,
                _completedEventCount);

            Assert.AreEqual(
                "sequence.test",
                _completedSequenceId);
        }

        [Test]
        public void Completion_PublishesSequenceCompletedEventOnce()
        {
            var blockingStep =
                new TestSequenceStep(
                    completeOnEnter: false);

            SequenceRuntime sequence =
                CreateSequence(
                    blockingStep);

            _player.Play(
                sequence);

            blockingStep.Complete();

            _player.Advance();
            _player.Advance();

            Assert.AreEqual(
                1,
                _completedEventCount);
        }

        [Test]
        public void Completion_ClearsActiveSequenceBeforePublishingEvent()
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
                SequenceRuntime sequence =
                    CreateSequence(
                        new TestSequenceStep(
                            completeOnEnter: true));

                _player.Play(
                    sequence);

                Assert.IsFalse(
                    wasPlayingDuringEvent);
            }
            finally
            {
                EventBus.Unsubscribe<SequenceCompletedEvent>(
                    OnCompleted);
            }
        }

        private SequenceRuntime CreateSequence(
            params ISequenceStep[] steps)
        {
            return CreateSequence(
                "sequence.test",
                steps);
        }

        private SequenceRuntime CreateSequence(
            string sequenceId,
            params ISequenceStep[] steps)
        {
            var definition =
                new SequenceDefinition(
                    sequenceId);

            return new SequenceRuntime(
                definition,
                new List<ISequenceStep>(
                    steps));
        }

        private void OnSequenceCompleted(
            SequenceCompletedEvent eventData)
        {
            _completedEventCount++;

            _completedSequenceId =
                eventData.SequenceId;
        }

        private sealed class TestSequenceStep :
            ISequenceStep
        {
            private readonly bool _completeOnEnter;

            public bool IsCompleted { get; private set; }

            public int EnterCount { get; private set; }

            public TestSequenceStep(
                bool completeOnEnter)
            {
                _completeOnEnter =
                    completeOnEnter;
            }

            public void Enter()
            {
                EnterCount++;

                if (_completeOnEnter)
                {
                    IsCompleted =
                        true;
                }
            }

            public void Complete()
            {
                IsCompleted =
                    true;
            }
        }
    }
}