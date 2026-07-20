using System;
using System.IO;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Bootstrap.Context;
using DeviGames.Atlas.Core.Bootstrap.Steps;
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
using DeviGames.Atlas.Core.Triggers.Models;
using DeviGames.Atlas.Core.Triggers.Interfaces;
using DeviGames.Atlas.Core.Triggers.Systems;
using DeviGames.Atlas.Dev.Hub.Services;
using DeviGames.Atlas.Gameplay.Inventory.Services;
using DeviGames.Atlas.Gameplay.Objectives.Services;
using UnityEngine;

namespace DeviGames.Playground.Bootstrap
{
    public sealed class RegisterPlaygroundServicesStep :
        IBootstrapStep
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

            var systemCollection =
                new SystemCollection();

            var executionService =
                new ExecutionService(
                    systemCollection);

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

            context.Services.Register(
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

            context.Services.Register<ISystemCollection>(
                systemCollection);

            context.Services.Register<IExecutionService>(
                executionService);

            RegisterTriggerRuntime(
                context,
                systemCollection);

            return Task.CompletedTask;
        }

        private static void RegisterTriggerRuntime(
            BootstrapContext context,
            ISystemCollection systemCollection)
        {
            var triggerCollection =
                new TriggerCollection();

            ServiceRegistry resolver =
                context.Services;

            var triggerContext =
                new TriggerContext(
                    resolver);

            var triggerRunner =
                new TriggerRunner(
                    triggerCollection,
                    triggerContext);

            context.Services.Register<ITriggerCollection>(
                triggerCollection);

            systemCollection.Add(
                triggerRunner);
        }
    }
}