using System;
using System.Collections.Generic;

namespace DeviGames.Atlas.Core.Sequence.Models
{
    [Serializable]
    public sealed class SequenceDefinition
    {
        public string Id;

        public List<SequenceStepDefinition> Steps =
            new();

        public SequenceDefinition(
            string id)
        {
            Id = id;
        }
    }
}