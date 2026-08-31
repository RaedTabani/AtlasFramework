using DeviGames.Atlas.Core.Sequence.Models;

namespace DeviGames.Atlas.Core.Sequence.Interfaces
{
    public interface ISequenceStepFactory
    {
        string Type { get; }

        ISequenceStep Create(
            SequenceStepDefinition definition);
    }
}