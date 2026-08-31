using DeviGames.Atlas.Core.Sequence.Models;
using DeviGames.Atlas.Core.Sequence.Services;

namespace DeviGames.Atlas.Core.Sequence.Interfaces
{
    public interface ISequenceFactory
    {
        SequenceRuntime Create(
            SequenceDefinition definition);
    }
}