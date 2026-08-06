using System;
using DeviGames.Atlas.Core.Objectives.Interfaces;
using DeviGames.Atlas.Core.Objectives.Models;
using DeviGames.Atlas.Core.Objectives.Runtime;

namespace DeviGames.Atlas.Core.Objectives.Factories
{
    public sealed class ObjectiveFactory :
        IObjectiveFactory
    {
        public ObjectiveRuntime Create(
            ObjectiveDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            return new ObjectiveRuntime(
                definition);
        }
    }
}