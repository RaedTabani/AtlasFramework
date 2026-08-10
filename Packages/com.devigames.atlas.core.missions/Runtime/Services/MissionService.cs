using System;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Lifecycle.Interfaces;
using DeviGames.Atlas.Core.Missions.Events;
using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Missions.Models;
using DeviGames.Atlas.Core.Missions.Runtime;
using DeviGames.Atlas.Core.Objectives.Events;

namespace DeviGames.Atlas.Core.Missions.Services
{
    public sealed class MissionService :
        IInitializable,
        IShutdownable
    {
        private readonly IMissionFactory _factory;
        private readonly IMissionCollection _collection;

        public string CurrentMission ="null"; 

        public MissionService(
            IMissionFactory factory,
            IMissionCollection collection)
        {
            _factory =
                factory
                ?? throw new ArgumentNullException(
                    nameof(factory));

            _collection =
                collection
                ?? throw new ArgumentNullException(
                    nameof(collection));
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

        public MissionRuntime Register(
            MissionDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            MissionRuntime runtime =
                _factory.Create(
                    definition);

            _collection.Add(
                runtime);

            CurrentMission = runtime.Id;

            return runtime;
        }

        public MissionRuntime Get(
            string missionId)
        {
            return _collection.Get(
                missionId);
        }

        public bool TryGet(
            string missionId,
            out MissionRuntime mission)
        {
            return _collection.TryGet(
                missionId,
                out mission);
        }

        public void Clear()
        {
            _collection.Clear();
        }

        private void OnObjectiveCompleted(
            ObjectiveCompletedEvent eventData)
        {
            var missions =
                _collection.Missions;

            for (int index = 0;
                 index < missions.Count;
                 index++)
            {
                MissionRuntime mission =
                    missions[index];

                MissionUpdateResult result =
                    mission.NotifyObjectiveCompleted(
                        eventData.ObjectiveId);

                switch (result)
                {
                    case MissionUpdateResult.None:
                        break;

                    case MissionUpdateResult.ObjectiveCompleted:

                        EventBus.Publish(
                            new MissionObjectiveCompletedEvent(
                                mission.Id,
                                eventData.ObjectiveId,
                                mission.CompletedObjectiveCount,
                                mission.ObjectiveCount));

                        break;

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

                        break;
                }
            }
        }
    }
}