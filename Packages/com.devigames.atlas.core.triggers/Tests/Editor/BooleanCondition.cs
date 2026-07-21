using DeviGames.Atlas.Core.Triggers.Interfaces;
using DeviGames.Atlas.Core.Triggers.Models;

namespace DeviGames.Atlas.Core.Triggers.Test
{
    public sealed class BooleanCondition :
        ITriggerCondition
    {
        private readonly bool _value;

        public BooleanCondition(
            bool value)
        {
            _value = value;
        }

        public bool Evaluate(
            TriggerContext context)
        {
            return _value;
        }
    }
}