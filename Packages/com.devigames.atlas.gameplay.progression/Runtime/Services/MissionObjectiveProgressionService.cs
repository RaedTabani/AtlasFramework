using System;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Missions.Models;
using DeviGames.Atlas.Core.Missions.Runtime;
using DeviGames.Atlas.Core.Objectives.Events;
using DeviGames.Atlas.Gameplay.Progression.Interfaces;

namespace DeviGames.Atlas.Gameplay.Progression.Services
{
    public sealed class MissionObjectiveProgressionService :
        IMissionObjectiveProgressionService,
        IInitializable,
        IShutdownable
    {
        private readonly IMissionSessionService _sessionService;
        private readonly IMissionCollection _missionCollection;

        public MissionObjectiveProgressionService(
            IMissionSessionService sessionService,
            IMissionCollection missionCollection)
        {
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
            _missionCollection = missionCollection ?? throw new ArgumentNullException(nameof(missionCollection));
        }

        public void Initialize()
        {
            EventBus.Subscribe<ObjectiveCompletedEvent>(
                OnObjectiveCompleted);
        }

        public void Shutdown()
        {
            EventBus.Unsubscribe<ObjectiveCompletedEvent>(
                OnObjectiveCompleted);
        }

        public void OnObjectiveCompleted(
            ObjectiveCompletedEvent eventData)
        {
            if (!_sessionService.HasActiveSession)
            {
                return;
            }

            if (!_missionCollection.TryGet(
                    _sessionService.ActiveMissionId,
                    out MissionRuntime mission))
            {
                return;
            }

            MissionUpdateResult result =
                mission.NotifyObjectiveCompleted(
                    eventData.ObjectiveId);

            switch (result)
            {
                case MissionUpdateResult.None:
                    return;

                case MissionUpdateResult.ObjectiveCompleted:

                    EventBus.Publish(
                        new MissionObjectiveCompletedEvent(
                            mission.Id,
                            eventData.ObjectiveId,
                            mission.CompletedObjectiveCount,
                            mission.ObjectiveCount));

                    return;

                case MissionUpdateResult.Completed:

                    EventBus.Publish(
                        new MissionObjectiveCompletedEvent(
                            mission.Id,
                            eventData.ObjectiveId,
                            mission.CompletedObjectiveCount,
                            mission.ObjectiveCount));

                    EventBus.Publish(
                        new MissionCompletedEvent(
                            mission.Id));

                    return;
            }
        }
    }
}