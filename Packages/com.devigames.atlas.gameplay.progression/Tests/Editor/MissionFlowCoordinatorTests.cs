using NUnit.Framework;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.GameFlow.Models;
using DeviGames.Atlas.Core.GameFlow.Services;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Gameplay.Progression.Interfaces;
using DeviGames.Atlas.Gameplay.Progression.Services;
using DeviGames.Atlas.Gameplay.Progression.Models;

namespace DeviGames.Atlas.Gameplay.Progression.Tests
{
    public sealed class MissionFlowCoordinatorTests
    {
        private FakeMissionSessionService _sessionService;
        private FakeMissionResultService _resultService;
        private IGameFlowService _gameFlowService;
        private MissionFlowCoordinator _coordinator;

        [SetUp]
        public void SetUp()
        {
            _sessionService =
                new FakeMissionSessionService();

            _resultService =
                new FakeMissionResultService();
                
            _gameFlowService =
                new GameFlowService();

            _coordinator =
                new MissionFlowCoordinator(
                    _sessionService,
                    _gameFlowService,
                    _resultService);

            _coordinator.Initialize();

            _gameFlowService.EnterMainMenu();
        }

        [TearDown]
        public void TearDown()
        {
            _coordinator.Shutdown();
        }

        [Test]
        public void NewCoordinator_HasNoMission()
        {
            Assert.That(
                _coordinator.HasMission,
                Is.False);

            Assert.That(
                _coordinator.MissionId,
                Is.Empty);
        }

        [Test]
        public void StartMission_WhenSessionStarts_EntersMissionIntro()
        {
            bool result =
                _coordinator.StartMission(
                    "mission.test");

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _sessionService.ActiveMissionId,
                Is.EqualTo(
                    "mission.test"));

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MissionIntro));
        }

        [Test]
        public void StartMission_WhenSuccessful_StoresMissionId()
        {
            bool result =
                _coordinator.StartMission(
                    "mission.test");

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _coordinator.HasMission,
                Is.True);

            Assert.That(
                _coordinator.MissionId,
                Is.EqualTo(
                    "mission.test"));
        }

        [Test]
        public void StartMission_WhenSessionFails_DoesNotChangeGameFlow()
        {
            _sessionService.AllowStart =
                false;

            bool result =
                _coordinator.StartMission(
                    "mission.test");

            Assert.That(
                result,
                Is.False);

            Assert.That(
                _sessionService.HasActiveSession,
                Is.False);

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MainMenu));
        }

        [Test]
        public void StartMission_WhenSessionFails_DoesNotStoreMissionId()
        {
            _sessionService.AllowStart =
                false;

            bool result =
                _coordinator.StartMission(
                    "mission.test");

            Assert.That(
                result,
                Is.False);

            Assert.That(
                _coordinator.HasMission,
                Is.False);

            Assert.That(
                _coordinator.MissionId,
                Is.Empty);
        }

        [Test]
        public void StartMission_WhenGameFlowRejectsIntro_RollsBackSession()
        {
            _gameFlowService.BeginMissionIntro();
            _gameFlowService.BeginGameplay();

            bool result =
                _coordinator.StartMission(
                    "mission.test");

            Assert.That(
                result,
                Is.False);

            Assert.That(
                _sessionService.ExitCallCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _sessionService.HasActiveSession,
                Is.False);

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.Gameplay));
        }

        [Test]
        public void StartMission_WhenGameFlowRejectsIntro_ClearsMissionId()
        {
            _gameFlowService.BeginMissionIntro();
            _gameFlowService.BeginGameplay();

            bool result =
                _coordinator.StartMission(
                    "mission.test");

            Assert.That(
                result,
                Is.False);

            Assert.That(
                _coordinator.HasMission,
                Is.False);

            Assert.That(
                _coordinator.MissionId,
                Is.Empty);
        }

        [Test]
        public void CancelMissionLaunch_WhenMissionIsInIntro_ReturnsToMainMenu()
        {
            _coordinator.StartMission(
                "mission.test");

            bool result =
                _coordinator.CancelMissionLaunch();

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MainMenu));
        }

        [Test]
        public void CancelMissionLaunch_WhenMissionIsInIntro_ExitsSession()
        {
            _coordinator.StartMission(
                "mission.test");

            bool result =
                _coordinator.CancelMissionLaunch();

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _sessionService.ExitCallCount,
                Is.EqualTo(
                    1));

            Assert.That(
                _sessionService.HasActiveSession,
                Is.False);

            Assert.That(
                _sessionService.ActiveMissionId,
                Is.Empty);
        }

        [Test]
        public void CancelMissionLaunch_WhenMissionIsInIntro_ClearsMissionId()
        {
            _coordinator.StartMission(
                "mission.test");

            bool result =
                _coordinator.CancelMissionLaunch();

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _coordinator.HasMission,
                Is.False);

            Assert.That(
                _coordinator.MissionId,
                Is.Empty);
        }

        [Test]
        public void CancelMissionLaunch_WhenNoMissionExists_Fails()
        {
            bool result =
                _coordinator.CancelMissionLaunch();

            Assert.That(
                result,
                Is.False);

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MainMenu));

            Assert.That(
                _sessionService.ExitCallCount,
                Is.EqualTo(
                    0));
        }

        [Test]
        public void CancelMissionLaunch_OutsideMissionIntro_Fails()
        {
            _coordinator.StartMission(
                "mission.test");

            _coordinator.CompleteIntro();

            bool result =
                _coordinator.CancelMissionLaunch();

            Assert.That(
                result,
                Is.False);

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.Gameplay));

            Assert.That(
                _sessionService.HasActiveSession,
                Is.True);

            Assert.That(
                _coordinator.MissionId,
                Is.EqualTo(
                    "mission.test"));

            Assert.That(
                _sessionService.ExitCallCount,
                Is.EqualTo(
                    0));
        }

        [Test]
        public void CancelMissionLaunch_AfterSuccessfulRollback_CanStartMissionAgain()
        {
            _coordinator.StartMission(
                "mission.test");

            bool cancelled =
                _coordinator.CancelMissionLaunch();

            bool restarted =
                _coordinator.StartMission(
                    "mission.test");

            Assert.That(
                cancelled,
                Is.True);

            Assert.That(
                restarted,
                Is.True);

            Assert.That(
                _sessionService.HasActiveSession,
                Is.True);

            Assert.That(
                _sessionService.ActiveMissionId,
                Is.EqualTo(
                    "mission.test"));

            Assert.That(
                _coordinator.MissionId,
                Is.EqualTo(
                    "mission.test"));

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MissionIntro));
        }

        [Test]
        public void CompleteIntro_FromMissionIntro_EntersGameplay()
        {
            _coordinator.StartMission(
                "mission.test");

            bool result =
                _coordinator.CompleteIntro();

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.Gameplay));
        }

        [Test]
        public void CompleteIntro_FromWrongState_Fails()
        {
            bool result =
                _coordinator.CompleteIntro();

            Assert.That(
                result,
                Is.False);

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MainMenu));
        }

        [Test]
        public void MissionCompleted_DuringGameplay_EntersMissionOutro()
        {
            _coordinator.StartMission(
                "mission.test");

            _coordinator.CompleteIntro();

            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.test"));

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MissionOutro));
        }

        [Test]
        public void MissionCompleted_DuringGameplay_PreservesMissionId()
        {
            MoveToMissionOutro();

            Assert.That(
                _coordinator.HasMission,
                Is.True);

            Assert.That(
                _coordinator.MissionId,
                Is.EqualTo(
                    "mission.test"));
        }

        [Test]
        public void MissionCompleted_ForDifferentMission_DoesNotEnterMissionOutro()
        {
            _coordinator.StartMission(
                "mission.test");

            _coordinator.CompleteIntro();

            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.other"));

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.Gameplay));

            Assert.That(
                _coordinator.MissionId,
                Is.EqualTo(
                    "mission.test"));
        }

        [Test]
        public void MissionCompleted_OutsideGameplay_DoesNotChangeState()
        {
            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.test"));

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MainMenu));
        }

        [Test]
        public void CompleteOutro_FromMissionOutro_EntersMissionResults()
        {
            MoveToMissionOutro();

            bool result =
                _coordinator.CompleteOutro();

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MissionResults));
        }

        [Test]
        public void CompleteOutro_PreservesMissionId()
        {
            MoveToMissionOutro();

            bool result =
                _coordinator.CompleteOutro();

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _coordinator.HasMission,
                Is.True);

            Assert.That(
                _coordinator.MissionId,
                Is.EqualTo(
                    "mission.test"));
        }

        [Test]
        public void CompleteResults_FromMissionResults_ReturnsToMainMenu()
        {
            MoveToMissionResults();

            bool result =
                _coordinator.CompleteResults();

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MainMenu));
        }

        [Test]
        public void CompleteResults_WhenSuccessful_ClearsMissionId()
        {
            MoveToMissionResults();

            bool result =
                _coordinator.CompleteResults();

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _coordinator.HasMission,
                Is.False);

            Assert.That(
                _coordinator.MissionId,
                Is.Empty);
        }

        [Test]
        public void CompleteResults_FromWrongState_DoesNotClearMissionId()
        {
            _coordinator.StartMission(
                "mission.test");

            bool result =
                _coordinator.CompleteResults();

            Assert.That(
                result,
                Is.False);

            Assert.That(
                _coordinator.HasMission,
                Is.True);

            Assert.That(
                _coordinator.MissionId,
                Is.EqualTo(
                    "mission.test"));

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MissionIntro));
        }

        [Test]
        public void Shutdown_UnsubscribesFromMissionCompletedEvent()
        {
            _coordinator.StartMission(
                "mission.test");

            _coordinator.CompleteIntro();

            _coordinator.Shutdown();

            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.test"));

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.Gameplay));

            _coordinator.Initialize();
        }
        [Test]
        public void StartMission_ClearsPreviousResult()
        {
            _resultService.SetResult(
                "mission.previous");

            bool result =
                _coordinator.StartMission(
                    "mission.test");

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _resultService.HasResult,
                Is.False);

            Assert.That(
                _resultService.ClearCallCount,
                Is.EqualTo(
                    1));
        }

        [Test]
        public void StartMission_WhenSessionFails_StillClearsPreviousResult()
        {
            _resultService.SetResult(
                "mission.previous");

            _sessionService.AllowStart =
                false;

            bool result =
                _coordinator.StartMission(
                    "mission.test");

            Assert.That(
                result,
                Is.False);

            Assert.That(
                _resultService.HasResult,
                Is.False);

            Assert.That(
                _resultService.ClearCallCount,
                Is.EqualTo(
                    1));
        }

        [Test]
        public void CancelMissionLaunch_DoesNotClearResultAgain()
        {
            _coordinator.StartMission(
                "mission.test");

            int clearCallCount =
                _resultService.ClearCallCount;

            bool result =
                _coordinator.CancelMissionLaunch();

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _resultService.ClearCallCount,
                Is.EqualTo(
                    clearCallCount));
        }
        private void MoveToMissionOutro()
        {
            _coordinator.StartMission(
                "mission.test");

            _coordinator.CompleteIntro();

            EventBus.Publish(
                new MissionCompletedEvent(
                    "mission.test"));
        }

        [Test]
        public void CompleteResults_DoesNotClearResult()
        {
            MoveToMissionResults();

            _resultService.SetResult(
                "mission.test");

            int clearCallCount =
                _resultService.ClearCallCount;

            bool result =
                _coordinator.CompleteResults();

            Assert.That(
                result,
                Is.True);

            Assert.That(
                _resultService.HasResult,
                Is.True);

            Assert.That(
                _resultService.ClearCallCount,
                Is.EqualTo(
                    clearCallCount));
        }
        [Test]
        public void MissionFailed_DuringGameplay_EntersMissionOutro()
        {
            _coordinator.StartMission(
                "mission.test");

            _coordinator.CompleteIntro();

            EventBus.Publish(
                new MissionFailedEvent(
                    "mission.test"));

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MissionOutro));
        }

        [Test]
        public void MissionFailed_DuringGameplay_PreservesMissionId()
        {
            _coordinator.StartMission(
                "mission.test");

            _coordinator.CompleteIntro();

            EventBus.Publish(
                new MissionFailedEvent(
                    "mission.test"));

            Assert.That(
                _coordinator.HasMission,
                Is.True);

            Assert.That(
                _coordinator.MissionId,
                Is.EqualTo(
                    "mission.test"));
        }

        [Test]
        public void MissionFailed_ForDifferentMission_DoesNotEnterMissionOutro()
        {
            _coordinator.StartMission(
                "mission.test");

            _coordinator.CompleteIntro();

            EventBus.Publish(
                new MissionFailedEvent(
                    "mission.other"));

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.Gameplay));

            Assert.That(
                _coordinator.MissionId,
                Is.EqualTo(
                    "mission.test"));
        }

        [Test]
        public void MissionFailed_OutsideGameplay_DoesNotChangeState()
        {
            _coordinator.StartMission(
                "mission.test");

            EventBus.Publish(
                new MissionFailedEvent(
                    "mission.test"));

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.MissionIntro));

            Assert.That(
                _coordinator.MissionId,
                Is.EqualTo(
                    "mission.test"));
        }

        [Test]
        public void Shutdown_UnsubscribesFromMissionFailedEvent()
        {
            _coordinator.StartMission(
                "mission.test");

            _coordinator.CompleteIntro();

            _coordinator.Shutdown();

            EventBus.Publish(
                new MissionFailedEvent(
                    "mission.test"));

            Assert.That(
                _gameFlowService.State,
                Is.EqualTo(
                    GameFlowState.Gameplay));

            _coordinator.Initialize();
        }
        private void MoveToMissionResults()
        {
            MoveToMissionOutro();

            _coordinator.CompleteOutro();
        }

        private sealed class FakeMissionSessionService :
            IMissionSessionService
        {
            public string ActiveMissionId { get; private set; } =
                string.Empty;

            public bool HasActiveSession =>
                !string.IsNullOrWhiteSpace(
                    ActiveMissionId);

            public bool AllowStart { get; set; } =
                true;

            public int ExitCallCount { get; private set; }

            public bool Start(
                string missionId)
            {
                if (!AllowStart)
                {
                    return false;
                }

                if (HasActiveSession)
                {
                    return false;
                }

                ActiveMissionId =
                    missionId;

                return true;
            }

            public bool Restart()
            {
                return HasActiveSession;
            }

            public bool Fail()
            {
                if (!HasActiveSession)
                {
                    return false;
                }

                ActiveMissionId =
                    string.Empty;

                return true;
            }

            public bool Exit()
            {
                ExitCallCount++;

                if (!HasActiveSession)
                {
                    return false;
                }

                ActiveMissionId =
                    string.Empty;

                return true;
            }
        }

        private sealed class FakeMissionResultService :
            IMissionResultService
        {
            public MissionResult CurrentResult { get; private set; }

            public bool HasResult =>
                CurrentResult != null;

            public int ClearCallCount { get; private set; }

            public bool TryGetResult(
                out MissionResult result)
            {
                result =
                    CurrentResult;

                return result != null;
            }

            public void Clear()
            {
                ClearCallCount++;

                CurrentResult =
                    null;
            }

            public void SetResult(
                string missionId)
            {
                CurrentResult =
                    new MissionResult(
                        missionId,
                        true);
            }
        }
    }
}