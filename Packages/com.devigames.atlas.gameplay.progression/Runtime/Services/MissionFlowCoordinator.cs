using System;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.GameFlow.Models;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Gameplay.Progression.Interfaces;

namespace DeviGames.Atlas.Gameplay.Progression.Services
{
    public sealed class MissionFlowCoordinator :
        IInitializable,
        IShutdownable
    {
        private readonly IMissionSessionService _sessionService;
        private readonly IGameFlowService _gameFlowService;
        private readonly IMissionResultService _resultService;

        public string MissionId { get; private set; }

        public bool HasMission =>
            !string.IsNullOrWhiteSpace(MissionId);

        public MissionFlowCoordinator(
            IMissionSessionService sessionService,
            IGameFlowService gameFlowService,
            IMissionResultService resultService)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _gameFlowService = gameFlowService ?? throw new ArgumentNullException(nameof(gameFlowService));
            _resultService = resultService ?? throw new ArgumentNullException(nameof(resultService));

            MissionId =
                string.Empty;
        }

        public void Initialize()
        {
            EventBus.Subscribe<MissionCompletedEvent>(
                OnMissionCompleted);
        }

        public void Shutdown()
        {
            EventBus.Unsubscribe<MissionCompletedEvent>(
                OnMissionCompleted);
        }

        public bool StartMission(
            string missionId)
        {
            _resultService.Clear();

            if (!_sessionService.Start(
                    missionId))
            {
                return false;
            }

            MissionId =
                missionId;

            if (_gameFlowService.BeginMissionIntro())
            {
                return true;
            }

            _sessionService.Exit();

            MissionId =
                string.Empty;

            return false;
        }

        public bool CancelMissionLaunch()
        {
            if (!HasMission)
            {
                return false;
            }

            if (_gameFlowService.State !=
                GameFlowState.MissionIntro)
            {
                return false;
            }

            if (!_gameFlowService.CancelMissionIntro())
            {
                return false;
            }

            _sessionService.Exit();

            MissionId =
                string.Empty;

            return true;
        }

        public bool CompleteIntro()
        {
            return _gameFlowService.BeginGameplay();
        }

        public bool CompleteOutro()
        {
            return _gameFlowService.BeginMissionResults();
        }

        public bool CompleteResults()
        {
            if (!_gameFlowService.EnterMainMenu())
            {
                return false;
            }

            MissionId =
                string.Empty;

            return true;
        }

        private void OnMissionCompleted(
            MissionCompletedEvent eventData)
        {
            if (_gameFlowService.State !=
                GameFlowState.Gameplay)
            {
                return;
            }

            if (!string.Equals(
                    MissionId,
                    eventData.MissionId,
                    StringComparison.Ordinal))
            {
                return;
            }

            _gameFlowService.BeginMissionOutro();
        }
    }
}