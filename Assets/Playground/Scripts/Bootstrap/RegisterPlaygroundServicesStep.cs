using System;
using System.IO;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Diagnostics.Save;
using DeviGames.Atlas.Core.Diagnostics.Services;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Execution.Services;
using DeviGames.Atlas.Core.Execution.Systems;
using DeviGames.Atlas.Core.Execution.Interfaces;
using DeviGames.Atlas.Core.Interaction.Services;
using DeviGames.Atlas.Core.Missions.Services;
using DeviGames.Atlas.Core.Objectives.Services;
using DeviGames.Atlas.Core.Progress.Services;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Save.Storage;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Triggers.Registry;
using DeviGames.Atlas.Core.Triggers.Factories;
using DeviGames.Atlas.Core.Triggers.Models;
using DeviGames.Atlas.Core.Triggers.Interfaces;
using DeviGames.Atlas.Core.Triggers.Systems;
using DeviGames.Atlas.Core.Triggers.Runtime;
using DeviGames.Atlas.Dev.Hub.Services;
using DeviGames.Atlas.Gameplay.Inventory.Services;
using DeviGames.Atlas.Gameplay.Inventory.Interfaces;
using DeviGames.Atlas.Gameplay.Objectives.Services;
using DeviGames.Atlas.Gameplay.Inventory.Triggers;
using DeviGames.Atlas.Unity.Execution.Installation;

using UnityEngine;

namespace DeviGames.Playground.Bootstrap
{
    public sealed class RegisterPlaygroundServicesStep : IBootstrapStep
    {
        public string Name =>
            "Register Playground Services";

        public Task ExecuteAsync(
            BootstrapContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(
                    nameof(context));
            }

            var installationContext =
                            new AtlasInstallationContext(
                                context.Services);

                        new ExecutionInstaller().Install(
                            installationContext);



            string savePath =
                Path.Combine(
                    UnityEngine.Application.persistentDataPath,
                    "DeviGames",
                    "Playground",
                    "Saves");

            

            var eventHistoryService =
                new EventHistoryService(250);

            var interactionService =
                new InteractionService();

            var inventoryService =
                new InventoryService();

            var objectiveService =
                new ObjectiveService();

            var progressService =
                new MissionProgressService();

            var diagnostics =
                new SaveDiagnosticsService(
                    savePath);

            


            var saveService =
                new SaveService(
                    new JsonFileSaveStorage(
                        savePath));

            var objectiveAdapter =
                new GameplayObjectiveAdapter(
                    objectiveService);

            var missionService =
                new MissionService(
                    objectiveService);

            var progressSaveCoordinator =
                new ProgressSaveCoordinator(
                    progressService,
                    saveService);

            var devHubSnapshotService =
                new DevHubSnapshotService(
                    missionService,
                    objectiveService,
                    progressService,
                    inventoryService);

            // Core event infrastructure must exist before
            // EventHistoryService is initialized.


            context.Services.Register(
                eventHistoryService);

            context.Services.Register(
                interactionService);

            context.Services.Register<IInventoryService>(
                inventoryService);

            context.Services.Register(
                objectiveService);

            context.Services.Register(
                progressService);

            context.Services.Register(
                saveService);

            context.Services.Register(
                objectiveAdapter);

            context.Services.Register(
                missionService);

            context.Services.Register(
                progressSaveCoordinator);

            context.Services.Register(
                devHubSnapshotService);

            context.Services.Register<ISaveDiagnosticsService>(
                diagnostics);





            RegisterTriggerRuntime(
                context,
                context.Services.Resolve<ISystemCollection>());

            return Task.CompletedTask;
        }

        private static void RegisterTriggerRuntime(
            BootstrapContext context,
            ISystemCollection systemCollection)
        {
            var triggerCollection =
                new TriggerCollection();

            var triggerContext =
                new TriggerContext(
                    context.Services);

            var triggerBuildContext =
                new TriggerBuildContext(
                    context.Services);

            var conditionFactoryRegistry =
                new TriggerConditionFactoryRegistry();

            var triggerFactory =
                new TriggerFactory(
                    conditionFactoryRegistry,
                    triggerBuildContext);

            var triggerRunner =
                new TriggerRunner(
                    triggerCollection,
                    triggerContext);


            context.Services.Register<ITriggerCollection>(
                triggerCollection);

            context.Services.Register<
                ITriggerConditionFactoryRegistry>(
                conditionFactoryRegistry);

            ITriggerConditionFactoryRegistry triggerConditionFactories =
                context.Services.Resolve<
                    ITriggerConditionFactoryRegistry>();
            
            triggerConditionFactories.Register(
                new InventoryQuantityConditionFactory());

            context.Services.Register<ITriggerFactory>(
                triggerFactory);

            systemCollection.Add(
                triggerRunner);



            var definition =
                new TriggerDefinition(
                    id: "playground.inventory.collect-three-keys",
                    repeatable: false,
                    condition:
                        new InventoryQuantityConditionDefinition(
                            itemId: "key",
                            requiredQuantity: 3));

            TriggerRuntime runtime =
                triggerFactory.Create(
                    definition);

            triggerCollection.Add(
                runtime);
        }
    }
}