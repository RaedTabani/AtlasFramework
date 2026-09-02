using DeviGames.Atlas.Core.Content.Models;
using DeviGames.Atlas.Core.Sequence.Models;

namespace DeviGames.Atlas.Core.Sequence.Interfaces
{
    public interface ISequenceStepContentConverter
    {
        string Type { get; }

        SequenceStepDefinition Convert(
            SequenceStepContentData data);
    }
}