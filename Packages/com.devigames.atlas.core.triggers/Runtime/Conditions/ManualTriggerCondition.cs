using DeviGames.Atlas.Core.Triggers.Models;
using DeviGames.Atlas.Core.Triggers.Interfaces;

namespace DeviGames.Atlas.Core.Triggers.Conditions
{
    public sealed class ManualTriggerCondition
        : ITriggerCondition
    {
        private bool _isSatisfied;

        public void Activate()
        {
            _isSatisfied = true;
        }

        public void Reset()
        {
            _isSatisfied = false;
        }

        public bool Evaluate(
            TriggerContext context)
        {
            return _isSatisfied;
        }
    }
}