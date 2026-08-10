using System;
using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Missions.Models;
using DeviGames.Atlas.Core.Missions.Runtime;

namespace DeviGames.Atlas.Core.Missions.Factories
{
    public sealed class MissionFactory :
        IMissionFactory
    {
        public MissionRuntime Create(
            MissionDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            return new MissionRuntime(
                definition);
        }
    }
}