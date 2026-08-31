using System;

namespace DeviGames.Atlas.Core.Sequence.Models
{
    [Serializable]
    public abstract class SequenceStepDefinition
    {
        public string Type;

        protected SequenceStepDefinition(
            string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException(
                    "Sequence step type cannot be null or empty.",
                    nameof(type));
            }

            Type = type;
        }
    }
}