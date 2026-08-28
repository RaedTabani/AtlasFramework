using NUnit.Framework;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.GameFlow.Events;
using DeviGames.Atlas.Core.GameFlow.Models;
using DeviGames.Atlas.Core.GameFlow.Services;

namespace DeviGames.Atlas.Core.GameFlow.Tests
{
    public sealed class GameFlowServiceTests
    {
        private GameFlowService _service;

        [SetUp]
        public void SetUp()
        {
            _service =
                new GameFlowService();
        }

        [Test]
        public void Constructor_StartsInBoot()
        {
            Assert.That(
                _service.State,
                Is.EqualTo(
                    GameFlowState.Boot));
        }

        [Test]
        public void EnterMainMenu_FromBoot_Succeeds()
        {
            bool result =
                _service.EnterMainMenu();

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _service.State,
                Is.EqualTo(
                    GameFlowState.MainMenu));
        }

        [Test]
        public void BeginMissionIntro_FromMainMenu_Succeeds()
        {
            _service.EnterMainMenu();

            bool result =
                _service.BeginMissionIntro();

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _service.State,
                Is.EqualTo(
                    GameFlowState.MissionIntro));
        }

        [Test]
        public void BeginGameplay_FromMissionIntro_Succeeds()
        {
            MoveToMissionIntro();

            bool result =
                _service.BeginGameplay();

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _service.State,
                Is.EqualTo(
                    GameFlowState.Gameplay));
        }

        [Test]
        public void BeginMissionOutro_FromGameplay_Succeeds()
        {
            MoveToGameplay();

            bool result =
                _service.BeginMissionOutro();

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _service.State,
                Is.EqualTo(
                    GameFlowState.MissionOutro));
        }

        [Test]
        public void BeginMissionResults_FromMissionOutro_Succeeds()
        {
            MoveToMissionOutro();

            bool result =
                _service.BeginMissionResults();

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _service.State,
                Is.EqualTo(
                    GameFlowState.MissionResults));
        }

        [Test]
        public void EnterMainMenu_FromMissionResults_Succeeds()
        {
            MoveToMissionResults();

            bool result =
                _service.EnterMainMenu();

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _service.State,
                Is.EqualTo(
                    GameFlowState.MainMenu));
        }

        [Test]
        public void BeginGameplay_FromMainMenu_Fails()
        {
            _service.EnterMainMenu();

            bool result =
                _service.BeginGameplay();

            Assert.That(
                result,
                Is.False);

            Assert.That(
                _service.State,
                Is.EqualTo(
                    GameFlowState.MainMenu));
        }

        [Test]
        public void SuccessfulTransition_PublishesStateChangedEvent()
        {
            GameFlowStateChangedEvent? received =
                null;

            void Handler(
                GameFlowStateChangedEvent eventData)
            {
                received =
                    eventData;
            }

            EventBus.Subscribe<GameFlowStateChangedEvent>(
                Handler);

            try
            {
                _service.EnterMainMenu();

                Assert.That(
                    received.HasValue,
                    Is.True);

                Assert.That(
                    received.Value.PreviousState,
                    Is.EqualTo(
                        GameFlowState.Boot));

                Assert.That(
                    received.Value.CurrentState,
                    Is.EqualTo(
                        GameFlowState.MainMenu));
            }
            finally
            {
                EventBus.Unsubscribe<GameFlowStateChangedEvent>(
                    Handler);
            }
        }

        [Test]
        public void InvalidTransition_DoesNotPublishStateChangedEvent()
        {
            int eventCount = 0;

            void Handler(
                GameFlowStateChangedEvent eventData)
            {
                eventCount++;
            }

            EventBus.Subscribe<GameFlowStateChangedEvent>(
                Handler);

            try
            {
                bool result =
                    _service.BeginGameplay();

                Assert.That(
                    result,
                    Is.False);

                Assert.That(
                    eventCount,
                    Is.Zero);
            }
            finally
            {
                EventBus.Unsubscribe<GameFlowStateChangedEvent>(
                    Handler);
            }
        }

        private void MoveToMissionIntro()
        {
            _service.EnterMainMenu();
            _service.BeginMissionIntro();
        }

        private void MoveToGameplay()
        {
            MoveToMissionIntro();
            _service.BeginGameplay();
        }

        private void MoveToMissionOutro()
        {
            MoveToGameplay();
            _service.BeginMissionOutro();
        }

        private void MoveToMissionResults()
        {
            MoveToMissionOutro();
            _service.BeginMissionResults();
        }
    }
}