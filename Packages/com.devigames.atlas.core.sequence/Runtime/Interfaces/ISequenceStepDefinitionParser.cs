using DeviGames.Atlas.Core.Sequence.Models;

using Newtonsoft.Json.Linq;

namespace DeviGames.Atlas.Core.Sequence.Interfaces
{
    public interface ISequenceStepDefinitionParser
    {
        string Type { get; }

        SequenceStepDefinition Parse(
            JObject json);
    }
}