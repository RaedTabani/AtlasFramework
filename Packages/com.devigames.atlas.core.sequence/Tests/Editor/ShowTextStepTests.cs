using System;

using DeviGames.Atlas.Core.Sequence.Interfaces;
using DeviGames.Atlas.Core.Sequence.Steps;

using NUnit.Framework;

namespace DeviGames.Atlas.Core.Sequence.Tests
{
    public sealed class ShowTextStepTests
    {
        [Test]
        public void Constructor_WithNullPresenter_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ShowTextStep(
                    "Hello",
                    null));
        }

        [Test]
        public void NewStep_IsNotCompleted()
        {
            var presenter =
                new TestSequenceTextPresenter();

            var step =
                new ShowTextStep(
                    "Hello",
                    presenter);

            Assert.IsFalse(
                step.IsCompleted);
        }

        [Test]
        public void Enter_ShowsText()
        {
            var presenter =
                new TestSequenceTextPresenter();

            var step =
                new ShowTextStep(
                    "Escape the house.",
                    presenter);

            step.Enter();

            Assert.AreEqual(
                "Escape the house.",
                presenter.LastText);
        }

        [Test]
        public void Enter_CompletesStep()
        {
            var presenter =
                new TestSequenceTextPresenter();

            var step =
                new ShowTextStep(
                    "Hello",
                    presenter);

            step.Enter();

            Assert.IsTrue(
                step.IsCompleted);
        }

        private sealed class TestSequenceTextPresenter :
            ISequenceTextPresenter
        {
            public string LastText { get; private set; }

            public void ShowText(
                string text)
            {
                LastText =
                    text;
            }
        }
    }
}