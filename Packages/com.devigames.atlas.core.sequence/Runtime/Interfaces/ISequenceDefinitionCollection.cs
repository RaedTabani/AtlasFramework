using System.Collections.Generic;

using DeviGames.Atlas.Core.Sequence.Models;

namespace DeviGames.Atlas.Core.Sequence.Interfaces
{
    public interface ISequenceDefinitionCollection
    {
        IReadOnlyCollection<SequenceDefinition> Definitions { get; }

        void Add(
            SequenceDefinition definition);

        bool TryGet(
            string sequenceId,
            out SequenceDefinition definition);

        SequenceDefinition Get(
            string sequenceId);
    }
}