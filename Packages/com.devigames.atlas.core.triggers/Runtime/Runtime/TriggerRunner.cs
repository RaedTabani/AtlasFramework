using System;
using UnityEngine;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Execution.Interfaces;
using DeviGames.Atlas.Core.Execution.Models;

using DeviGames.Atlas.Core.Triggers.Events;
using DeviGames.Atlas.Core.Triggers.Models;
using DeviGames.Atlas.Core.Triggers.Runtime;
using DeviGames.Atlas.Core.Triggers.Interfaces;

namespace DeviGames.Atlas.Core.Triggers.Systems
{
    public sealed class TriggerRunner :
        IUpdatable
    {
        private readonly ITriggerCollection _triggers;
        private readonly TriggerContext _triggerContext;

        public TriggerRunner(
            ITriggerCollection triggers,
            TriggerContext triggerContext)
        {
            _triggers =
                triggers
                ?? throw new ArgumentNullException(
                    nameof(triggers));

            _triggerContext =
                triggerContext
                ?? throw new ArgumentNullException(
                    nameof(triggerContext));
        }

        public void Update(
            ExecutionContext context)
        {
            var triggers =
                _triggers.Triggers;

            for (int index = 0;
                 index < triggers.Count;
                 index++)
            {
                TriggerRuntime trigger =
                    triggers[index];

                TriggerUpdateResult result =
                    trigger.Update(
                        _triggerContext);

                if (result != TriggerUpdateResult.Activated)
                    continue;

                EventBus.Publish(
                    new TriggerActivatedEvent(
                        trigger.Definition.Id));
            }
        }
    }
}