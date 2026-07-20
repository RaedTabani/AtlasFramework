using System;
using DeviGames.Atlas.Core.Triggers.Conditions;
using DeviGames.Atlas.Core.Triggers.Interfaces;
using DeviGames.Atlas.Core.Triggers.Models;


namespace DeviGames.Atlas.Core.Triggers.Runtime
{
    public sealed class TriggerRuntime
    {
        public TriggerDefinition Definition { get; }

        public ITriggerCondition Condition { get; }

        public TriggerState State { get; private set; }

        public TriggerRuntime(
            TriggerDefinition definition,
            ITriggerCondition condition)
        {
            Definition =
                definition
                ?? throw new ArgumentNullException(
                    nameof(definition));

            Condition =
                condition
                ?? throw new ArgumentNullException(
                    nameof(condition));

            State = TriggerState.Waiting;
        }

        public TriggerUpdateResult Update(
            TriggerContext context)
        {
            bool isSatisfied =
                Condition.Evaluate(context);

            if (State == TriggerState.Waiting)
            {
                if (!isSatisfied)
                    return TriggerUpdateResult.None;

                State = TriggerState.Activated;

                return TriggerUpdateResult.Activated;
            }

            if (!Definition.Repeatable)
                return TriggerUpdateResult.None;

            if (isSatisfied)
                return TriggerUpdateResult.None;

            State = TriggerState.Waiting;

            return TriggerUpdateResult.Reset;
        }
    }
}