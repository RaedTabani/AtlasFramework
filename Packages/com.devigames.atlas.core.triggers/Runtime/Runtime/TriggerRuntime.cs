using DeviGames.Atlas.Core.Triggers.Conditions;
using DeviGames.Atlas.Core.Triggers.Context;
using DeviGames.Atlas.Core.Triggers.Definition;
using DeviGames.Atlas.Core.Triggers.Models;

namespace DeviGames.Atlas.Core.Triggers.Runtime
{
    public sealed class TriggerRuntime
    {
        public TriggerDefinition Definition { get; }

        public TriggerState State { get; private set; }

        public bool IsActivated =>
            State == TriggerState.Activated;

        public TriggerRuntime(
            TriggerDefinition definition)
        {
            Definition = definition;

            State = TriggerState.Waiting;
        }

        public TriggerUpdateResult Update(
            TriggerContext context)
        {
            bool satisfied =
                Definition.Condition.Evaluate(context);

            if (!Definition.Repeatable)
            {
                if (State == TriggerState.Activated)
                    return TriggerUpdateResult.None;

                if (!satisfied)
                    return TriggerUpdateResult.None;

                State = TriggerState.Activated;

                return TriggerUpdateResult.Activated;
            }

            if (satisfied)
            {
                if (State == TriggerState.Waiting)
                {
                    State = TriggerState.Activated;
                    return TriggerUpdateResult.Activated;
                }
            }
            else
            {
                State = TriggerState.Waiting;
            }

            return TriggerUpdateResult.None;
        }
    }
}