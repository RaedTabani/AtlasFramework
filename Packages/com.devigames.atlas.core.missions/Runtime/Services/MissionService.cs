using System;

using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Missions.Runtime;
using DeviGames.Atlas.Core.Missions.Models;

namespace DeviGames.Atlas.Core.Missions.Services
{
    public sealed class MissionService
    {
        private readonly IMissionFactory _factory;
        private readonly IMissionCollection _collection;

        public MissionService(
            IMissionFactory factory,
            IMissionCollection collection)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
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
    }
}