using System;

namespace DeviGames.Atlas.Core.Content.Models
{
    [Serializable]
    public sealed class SequenceContentData
    {
        public string Id;

        public SequenceStepContentData[] Steps =
            Array.Empty<SequenceStepContentData>();
    }
}