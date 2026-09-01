using DeviGames.Atlas.Core.Sequence.Steps;

using NUnit.Framework;

namespace DeviGames.Atlas.Core.Sequence.Tests
{
    public sealed class WaitForContinueStepTests
    {
        private WaitForContinueStep _step;

        [SetUp]
        public void SetUp()
        {
            _step =
                new WaitForContinueStep();
        }

        [Test]
        public void NewStep_IsNotCompleted()
        {
            Assert.IsFalse(
                _step.IsCompleted);
        }

        [Test]
        public void Enter_DoesNotCompleteStep()
        {
            _step.Enter();

            Assert.IsFalse(
                _step.IsCompleted);
        }

        [Test]
        public void Continue_CompletesStep()
        {
            _step.Enter();

            bool result =
                _step.Continue();

            Assert.IsTrue(
                result);

            Assert.IsTrue(
                _step.IsCompleted);
        }

        [Test]
        public void Continue_WhenAlreadyCompleted_ReturnsFalse()
        {
            _step.Enter();

            _step.Continue();

            bool result =
                _step.Continue();

            Assert.IsFalse(
                result);

            Assert.IsTrue(
                _step.IsCompleted);
        }
    }
}