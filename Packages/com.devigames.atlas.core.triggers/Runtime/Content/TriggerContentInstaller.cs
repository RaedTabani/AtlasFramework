using System;

using DeviGames.Atlas.Core.Content.Models;
using DeviGames.Atlas.Core.Triggers.Interfaces;
using DeviGames.Atlas.Core.Triggers.Models;
using DeviGames.Atlas.Core.Triggers.Runtime;

namespace DeviGames.Atlas.Core.Triggers.Content
{
    public sealed class TriggerContentInstaller
    {
        private readonly TriggerContentConditionAdapterRegistry
            _adapterRegistry;

        private readonly ITriggerFactory
            _triggerFactory;

        private readonly ITriggerCollection
            _triggerCollection;

        public TriggerContentInstaller(
            TriggerContentConditionAdapterRegistry adapterRegistry,
            ITriggerFactory triggerFactory,
            ITriggerCollection triggerCollection)
        {
            _adapterRegistry =
                adapterRegistry
                ?? throw new ArgumentNullException(
                    nameof(adapterRegistry));

            _triggerFactory =
                triggerFactory
                ?? throw new ArgumentNullException(
                    nameof(triggerFactory));

            _triggerCollection =
                triggerCollection
                ?? throw new ArgumentNullException(
                    nameof(triggerCollection));
        }

        public void Install(
            ContentPackageData package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(
                    nameof(package));
            }

            if (package.Triggers == null)
            {
                return;
            }

            foreach (TriggerContentData data
                     in package.Triggers)
            {
                ITriggerContentConditionAdapter adapter =
                    _adapterRegistry.Resolve(
                        data.ConditionType);

                ITriggerConditionDefinition condition =
                    adapter.CreateDefinition(
                        data);

                var definition =
                    new TriggerDefinition(
                        id:
                            data.Id,
                        repeatable:
                            data.Repeatable,
                        condition:
                            condition);

                TriggerRuntime runtime =
                    _triggerFactory.Create(
                        definition);

                _triggerCollection.Add(
                    runtime);
            }
        }
    }
}