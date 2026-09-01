using System;
using System.Collections.Generic;

using DeviGames.Atlas.Core.Sequence.Interfaces;
using DeviGames.Atlas.Core.Sequence.Models;
using DeviGames.Atlas.Core.Sequence.Services;

using NUnit.Framework;

namespace DeviGames.Atlas.Core.Sequence.Tests
{
    public sealed class SequenceRuntimeTests
    {
        private SequenceDefinition _definition;

        [SetUp]
        public void SetUp()
        {
            _definition =
                new SequenceDefinition(
                    "sequence.test");
        }

        [Test]
        public void Constructor_SetsReadyState()
        {
            SequenceRuntime runtime =
                CreateRuntime();

            Assert.AreEqual(
                SequenceState.Ready,
                runtime.State);

            Assert.AreEqual(
                -1,
                runtime.CurrentStepIndex);

            Assert.IsNull(
                runtime.CurrentStep);

            Assert.IsFalse(
                runtime.HasCurrentStepEntered);
        }

        [Test]
        public void Constructor_WithNullDefinition_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => new SequenceRuntime(
                    null,
                    Array.Empty<ISequenceStep>()));
        }

        [Test]
        public void Constructor_WithNullSteps_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => new SequenceRuntime(
                    _definition,
                    null));
        }

        [Test]
        public void Constructor_WithEmptyId_Throws()
        {
            var definition =
                new SequenceDefinition(
                    string.Empty);

            Assert.Throws<ArgumentException>(
                () => new SequenceRuntime(
                    definition,
                    Array.Empty<ISequenceStep>()));
        }

        [Test]
        public void Start_WithSteps_SelectsFirstStep()
        {
            var firstStep =
                new TestSequenceStep();

            var secondStep =
                new TestSequenceStep();

            SequenceRuntime runtime =
                CreateRuntime(
                    firstStep,
                    secondStep);

            bool result =
                runtime.Start();

            Assert.IsTrue(
                result);

            Assert.AreEqual(
                SequenceState.Playing,
                runtime.State);

            Assert.AreEqual(
                0,
                runtime.CurrentStepIndex);

            Assert.AreSame(
                firstStep,
                runtime.CurrentStep);

            Assert.IsFalse(
                runtime.HasCurrentStepEntered);
        }

        [Test]
        public void Start_WithNoSteps_HasNoCurrentStep()
        {
            SequenceRuntime runtime =
                CreateRuntime();

            bool result =
                runtime.Start();

            Assert.IsTrue(
                result);

            Assert.AreEqual(
                SequenceState.Playing,
                runtime.State);

            Assert.AreEqual(
                -1,
                runtime.CurrentStepIndex);

            Assert.IsNull(
                runtime.CurrentStep);
        }

        [Test]
        public void Start_WhenAlreadyPlaying_ReturnsFalse()
        {
            SequenceRuntime runtime =
                CreateRuntime(
                    new TestSequenceStep());

            runtime.Start();

            bool result =
                runtime.Start();

            Assert.IsFalse(
                result);
        }

        [Test]
        public void EnterCurrentStep_EntersStepOnce()
        {
            var step =
                new TestSequenceStep();

            SequenceRuntime runtime =
                CreateRuntime(
                    step);

            runtime.Start();

            bool firstResult =
                runtime.EnterCurrentStep();

            bool secondResult =
                runtime.EnterCurrentStep();

            Assert.IsTrue(
                firstResult);

            Assert.IsFalse(
                secondResult);

            Assert.AreEqual(
                1,
                step.EnterCount);

            Assert.IsTrue(
                runtime.HasCurrentStepEntered);
        }

        [Test]
        public void EnterCurrentStep_BeforeStart_ReturnsFalse()
        {
            var step =
                new TestSequenceStep();

            SequenceRuntime runtime =
                CreateRuntime(
                    step);

            bool result =
                runtime.EnterCurrentStep();

            Assert.IsFalse(
                result);

            Assert.AreEqual(
                0,
                step.EnterCount);
        }

        [Test]
        public void MoveNext_WhenCurrentStepIsIncomplete_ReturnsFalse()
        {
            var step =
                new TestSequenceStep();

            SequenceRuntime runtime =
                CreateRuntime(
                    step);

            runtime.Start();
            runtime.EnterCurrentStep();

            bool result =
                runtime.MoveNext();

            Assert.IsFalse(
                result);

            Assert.AreEqual(
                0,
                runtime.CurrentStepIndex);
        }

        [Test]
        public void MoveNext_WhenCurrentStepIsCompleted_AdvancesToNextStep()
        {
            var firstStep =
                new TestSequenceStep();

            var secondStep =
                new TestSequenceStep();

            SequenceRuntime runtime =
                CreateRuntime(
                    firstStep,
                    secondStep);

            runtime.Start();
            runtime.EnterCurrentStep();

            firstStep.Complete();

            bool result =
                runtime.MoveNext();

            Assert.IsTrue(
                result);

            Assert.AreEqual(
                1,
                runtime.CurrentStepIndex);

            Assert.AreSame(
                secondStep,
                runtime.CurrentStep);

            Assert.IsFalse(
                runtime.HasCurrentStepEntered);
        }

        [Test]
        public void MoveNext_FromFinalCompletedStep_ReachesEnd()
        {
            var step =
                new TestSequenceStep();

            SequenceRuntime runtime =
                CreateRuntime(
                    step);

            runtime.Start();
            runtime.EnterCurrentStep();

            step.Complete();

            bool result =
                runtime.MoveNext();

            Assert.IsTrue(
                result);

            Assert.AreEqual(
                -1,
                runtime.CurrentStepIndex);

            Assert.IsNull(
                runtime.CurrentStep);

            Assert.IsFalse(
                runtime.HasCurrentStepEntered);
        }

        [Test]
        public void Complete_WhileCurrentStepExists_ReturnsFalse()
        {
            SequenceRuntime runtime =
                CreateRuntime(
                    new TestSequenceStep());

            runtime.Start();

            bool result =
                runtime.Complete();

            Assert.IsFalse(
                result);

            Assert.AreEqual(
                SequenceState.Playing,
                runtime.State);
        }

        [Test]
        public void Complete_AfterFinalStep_TransitionsToCompleted()
        {
            var step =
                new TestSequenceStep();

            SequenceRuntime runtime =
                CreateRuntime(
                    step);

            runtime.Start();
            runtime.EnterCurrentStep();

            step.Complete();

            runtime.MoveNext();

            bool result =
                runtime.Complete();

            Assert.IsTrue(
                result);

            Assert.AreEqual(
                SequenceState.Completed,
                runtime.State);
        }

        [Test]
        public void Complete_EmptySequence_TransitionsToCompleted()
        {
            SequenceRuntime runtime =
                CreateRuntime();

            runtime.Start();

            bool result =
                runtime.Complete();

            Assert.IsTrue(
                result);
        }

        [Test]
        public void Reset_FromCompleted_ReturnsToReady()
        {
            SequenceRuntime runtime =
                CreateRuntime();

            runtime.Start();
            runtime.Complete();

            runtime.Reset();

            Assert.AreEqual(
                SequenceState.Ready,
                runtime.State);

            Assert.AreEqual(
                -1,
                runtime.CurrentStepIndex);

            Assert.IsFalse(
                runtime.HasCurrentStepEntered);
        }

        [Test]
        public void Reset_AllowsSequenceToStartAgain()
        {
            SequenceRuntime runtime =
                CreateRuntime();

            runtime.Start();
            runtime.Complete();
            runtime.Reset();

            bool result =
                runtime.Start();

            Assert.IsTrue(
                result);

            Assert.AreEqual(
                SequenceState.Playing,
                runtime.State);
        }

        private SequenceRuntime CreateRuntime(
            params ISequenceStep[] steps)
        {
            return new SequenceRuntime(
                _definition,
                new List<ISequenceStep>(
                    steps));
        }

        private sealed class TestSequenceStep :
            ISequenceStep
        {
            public bool IsCompleted { get; private set; }

            public int EnterCount { get; private set; }

            public void Enter()
            {
                EnterCount++;
            }

            public void Complete()
            {
                IsCompleted =
                    true;
            }
        }
    }
}