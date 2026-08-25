using System;

using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Missions.Runtime;
using DeviGames.Atlas.Core.Objectives.Interfaces;
using DeviGames.Atlas.Core.Objectives.Runtime;
using DeviGames.Atlas.Gameplay.Progression.Interfaces;

namespace DeviGames.Atlas.Gameplay.Progression.Services
{
    public sealed class MissionSessionService :
        IMissionSessionService,
        IInitializable,
        IShutdownable
    {
        private readonly IMissionCollection _missionCollection;
        private readonly IObjectiveCollection _objectiveCollection;
        private readonly IMissionAvailabilityService _availabilityService;

        public string ActiveMissionId { get; private set; }

        public bool HasActiveSession =>
            !string.IsNullOrWhiteSpace(ActiveMissionId);

        public MissionSessionService(
            IMissionCollection missionCollection,
            IObjectiveCollection objectiveCollection,
            IMissionAvailabilityService availabilityService)
        {
            _missionCollection = missionCollection ?? throw new ArgumentNullException(nameof(missionCollection));
            _objectiveCollection = objectiveCollection ?? throw new ArgumentNullException(nameof(objectiveCollection));
            _availabilityService = availabilityService ?? throw new ArgumentNullException(nameof(availabilityService));

            ActiveMissionId = string.Empty;
        }

        public void Initialize()
        {
            EventBus.Subscribe<MissionCompletedEvent>(OnMissionCompleted);
        }

        public void Shutdown()
        {
            EventBus.Unsubscribe<MissionCompletedEvent>(OnMissionCompleted);
        }

        public bool Start(string missionId)
        {
            if (string.IsNullOrWhiteSpace(missionId))
            {
                return false;
            }

            if (HasActiveSession)
            {
                return false;
            }

            if (!_availabilityService.IsAvailable(missionId))
            {
                return false;
            }

            if (!_missionCollection.TryGet(missionId, out MissionRuntime mission))
            {
                return false;
            }

            ResetMissionRuntime(mission);

            ActiveMissionId = missionId;

            return true;
        }

        public bool Restart()
        {
            if (!HasActiveSession)
            {
                return false;
            }

            if (!_missionCollection.TryGet(ActiveMissionId, out MissionRuntime mission))
            {
                return false;
            }

            ResetMissionRuntime(mission);

            return true;
        }

        public bool Exit()
        {
            if (!HasActiveSession)
            {
                return false;
            }

            if (_missionCollection.TryGet(ActiveMissionId, out MissionRuntime mission))
            {
                ResetMissionRuntime(mission);
            }

            ActiveMissionId = string.Empty;

            return true;
        }

        private void OnMissionCompleted(MissionCompletedEvent eventData)
        {
            if (!HasActiveSession)
            {
                return;
            }

            if (!string.Equals(ActiveMissionId, eventData.MissionId, StringComparison.Ordinal))
            {
                return;
            }

            ActiveMissionId = string.Empty;
        }

        private void ResetMissionRuntime(MissionRuntime mission)
        {
            mission.Reset();

            for (int index = 0; index < mission.Definition.ObjectiveIds.Count; index++)
            {
                string objectiveId = mission.Definition.ObjectiveIds[index];

                if (!_objectiveCollection.TryGet(objectiveId, out ObjectiveRuntime objective))
                {
                    continue;
                }

                objective.Reset();
            }
        }
    }
}