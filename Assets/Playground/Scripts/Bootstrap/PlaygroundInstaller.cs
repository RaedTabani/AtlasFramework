using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Missions.Models;
using DeviGames.Atlas.Core.Missions.Services;
using DeviGames.Atlas.Core.Objectives.Models;
using DeviGames.Atlas.Core.Objectives.Services;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Triggers.Interfaces;
using DeviGames.Atlas.Core.Triggers.Models;
using DeviGames.Atlas.Core.Triggers.Runtime;
using DeviGames.Atlas.Gameplay.Inventory.Triggers;
using DeviGames.Atlas.Gameplay.Objectives.Models;
using DeviGames.Atlas.Gameplay.Objectives.Services;

namespace DeviGames.Playground.Bootstrap
{
    public sealed class PlaygroundInstaller :
        IAtlasInstaller
    {
        private const string CollectKeysObjectiveId =
            "objective.playground.collect-three-keys";

        private const string EscapeMissionId =
            "mission.playground.escape";

        private const string InventoryTriggerId =
            "playground.inventory.collect-three-keys";

        public void Install(AtlasInstallationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(
                    nameof(context));
            }

            ServiceRegistry services =
                context.Services;

            EnsureNotInstalled(services);

            InstallObjectives(services);

            InstallMissions(services);

            InstallTriggers(services);

            InstallBindings(services);
        }

        private static void InstallObjectives(ServiceRegistry services)
        {
            ObjectiveService objectiveService =
                services.Resolve<ObjectiveService>();

            var definition =
                new ObjectiveDefinition(
                    id:
                        CollectKeysObjectiveId,
                    displayName:
                        "Collect Three Keys",
                    description:
                        "Collect three keys.",
                    targetValue:
                        3);
                        
            var goldenKeyDefinition =
                new ObjectiveDefinition(
                    id:
                        "CollectGoldenKeyObjectiveId",
                    displayName:
                        "Collect Golden Key",
                    description:
                        "Collect Golden key.",
                    targetValue:
                        1);

            objectiveService.Register(
                definition);
            objectiveService.Register(
                goldenKeyDefinition);
        }

        private static void InstallMissions(
            ServiceRegistry services)
        {
            MissionService missionService =
                services.Resolve<MissionService>();

            var definition =
                new MissionDefinition(
                    id:
                        EscapeMissionId,
                    displayName:
                        "Escape the Playground",
                    description:
                        "Collect the keys needed to escape.",
                    objectiveIds:
                        new[]
                        {
                            CollectKeysObjectiveId,
                            "CollectGoldenKeyObjectiveId"
                        });

            missionService.Register(
                definition);
        }

        private static void InstallTriggers(
            ServiceRegistry services)
        {
            ITriggerFactory triggerFactory =
                services.Resolve<ITriggerFactory>();

            ITriggerCollection triggerCollection =
                services.Resolve<ITriggerCollection>();

            var definition =
                new TriggerDefinition(
                    id:
                        InventoryTriggerId,
                    repeatable:
                        false,
                    condition:
                        new InventoryQuantityConditionDefinition(
                            itemId:
                                "key",
                            requiredQuantity:
                                3));

            TriggerRuntime runtime =
                triggerFactory.Create(
                    definition);

            triggerCollection.Add(
                runtime);
        }

        private static void InstallBindings(
            ServiceRegistry services)
        {
            GameplayObjectiveAdapter adapter =
                services.Resolve<
                    GameplayObjectiveAdapter>();

            adapter.AddItemCollectedObjectiveBinding(
                new ItemCollectedObjectiveBinding(
                    objectiveId:
                        CollectKeysObjectiveId,
                    itemId:
                        "key",
                    progressAmount:
                        1));
        
            adapter.AddItemCollectedObjectiveBinding(
                new ItemCollectedObjectiveBinding(
                    objectiveId:
                        "CollectGoldenKeyObjectiveId",
                    itemId:
                        "golden_key",
                    progressAmount:
                        1));
        }

        private static void EnsureNotInstalled(
            ServiceRegistry services)
        {
            /*
             * We can make this stronger later.
             *
             * For now, Playground content itself will already
             * fail on duplicate objective / mission / trigger IDs.
             */
        }
    }
}