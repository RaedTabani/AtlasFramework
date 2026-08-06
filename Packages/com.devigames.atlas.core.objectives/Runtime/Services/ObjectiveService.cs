using System;

using DeviGames.Atlas.Core.Objectives.Events;
using DeviGames.Atlas.Core.Objectives.Interfaces;
using DeviGames.Atlas.Core.Objectives.Models;
using DeviGames.Atlas.Core.Objectives.Runtime;
using DeviGames.Atlas.Core.Events;

namespace DeviGames.Atlas.Core.Objectives.Services
{
    public sealed class ObjectiveService
    {
        private readonly IObjectiveFactory _factory;

        private readonly IObjectiveCollection _collection;

        public ObjectiveService(
            IObjectiveFactory factory,
            IObjectiveCollection collection)
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

        public ObjectiveRuntime Register(
            ObjectiveDefinition definition)
        {
            ObjectiveRuntime runtime =
                _factory.Create(
                    definition);

            _collection.Add(
                runtime);

            return runtime;
        }

        public ObjectiveUpdateResult AddProgress(
            string objectiveId,
            int amount)
        {
            ObjectiveRuntime runtime =
                _collection.Get(
                    objectiveId);

            int previousValue =
                runtime.CurrentValue;

            ObjectiveUpdateResult result =
                runtime.AddProgress(
                    amount);

            switch (result)
            {
                case ObjectiveUpdateResult.Progressed:

                    EventBus.Publish(
                        new ObjectiveProgressedEvent(
                            objectiveId,
                            previousValue,
                            runtime.CurrentValue,
                            runtime.TargetValue));

                    break;

                case ObjectiveUpdateResult.Completed:

                    EventBus.Publish(
                        new ObjectiveProgressedEvent(
                            objectiveId,
                            previousValue,
                            runtime.CurrentValue,
                            runtime.TargetValue));

                    EventBus.Publish(
                        new ObjectiveCompletedEvent(
                            objectiveId,
                            runtime.CurrentValue,
                            runtime.TargetValue));

                    break;
            }

            return result;
        }

        public ObjectiveRuntime Get(
            string objectiveId)
        {
            return _collection.Get(
                objectiveId);
        }

        public bool TryGet(
            string objectiveId,
            out ObjectiveRuntime runtime)
        {
            return _collection.TryGet(
                objectiveId,
                out runtime);
        }

        public bool IsCompleted(
            string objectiveId)
        {
            ObjectiveRuntime runtime =
                _collection.Get(
                    objectiveId);

            return runtime.IsCompleted;
        }

        public void Clear()
        {
            _collection.Clear();
        }
    }
}