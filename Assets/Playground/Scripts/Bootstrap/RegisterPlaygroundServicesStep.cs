using System;
using System.IO;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Content.Installation;
using DeviGames.Atlas.Core.Diagnostics.Save;
using DeviGames.Atlas.Core.Diagnostics.Services;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Execution.Services;
using DeviGames.Atlas.Core.Execution.Systems;
using DeviGames.Atlas.Core.Execution.Interfaces;
using DeviGames.Atlas.Core.Interaction.Services;
using DeviGames.Atlas.Core.Missions.Services;
using DeviGames.Atlas.Core.Missions.Installation;
using DeviGames.Atlas.Core.Missions.Interfaces;
using DeviGames.Atlas.Core.Objectives.Services;
using DeviGames.Atlas.Core.Objectives.Installation;
using DeviGames.Atlas.Core.Objectives.Interfaces;
using DeviGames.Atlas.Core.Progress.Services;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Save.Storage;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Triggers.Registry;
using DeviGames.Atlas.Core.Triggers.Factories;
using DeviGames.Atlas.Core.Triggers.Models;
using DeviGames.Atlas.Core.Triggers.Installation;
using DeviGames.Atlas.Core.Triggers.Interfaces;
using DeviGames.Atlas.Core.Triggers.Systems;
using DeviGames.Atlas.Core.Triggers.Runtime;
using DeviGames.Atlas.Dev.Hub.Services;
using DeviGames.Atlas.Gameplay.Inventory.Services;
using DeviGames.Atlas.Gameplay.Inventory.Interfaces;
using DeviGames.Atlas.Gameplay.Objectives.Services;
using DeviGames.Atlas.Gameplay.Objectives.Models;
using DeviGames.Atlas.Gameplay.Objectives.Content;
using DeviGames.Atlas.Gameplay.Inventory.Triggers;
using DeviGames.Atlas.Gameplay.Inventory.Installation;
using DeviGames.Atlas.Unity.Execution.Installation;

using UnityEngine;
using System.Collections.Generic;

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

            var installationContext = new AtlasInstallationContext(context.Services);

            new ExecutionInstaller().Install(installationContext);
            new TriggerInstaller().Install(installationContext);
            new InventoryInstaller().Install(installationContext);
            new ObjectiveInstaller().Install(installationContext);
            new MissionInstaller().Install(installationContext);
            new ContentInstaller().Install(installationContext);


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


            var progressService =
                new MissionProgressService();

            var diagnostics =
                new SaveDiagnosticsService(
                    savePath);

            
            ObjectiveService objectiveService =
                context.Services.Resolve<
                    ObjectiveService>();

            var saveService =
                new SaveService(
                    new JsonFileSaveStorage(
                        savePath));

            var objectiveAdapter =
                new GameplayObjectiveAdapter(
                    objectiveService);

            context.Services.Register(
                objectiveAdapter);

            new GameplayObjectiveContentIntegrationInstaller()
                .Install(installationContext);
            

            new PlaygroundInstaller().Install(installationContext);
          

            var progressSaveCoordinator =
                new ProgressSaveCoordinator(
                    progressService,
                    saveService);
            
            IMissionCollection missionCollection =
            context.Services.Resolve<
                IMissionCollection>();

            IObjectiveCollection objectiveCollection =
                context.Services.Resolve<
                    IObjectiveCollection>();

            IInventoryService inventoryService =
                context.Services.Resolve<
                    IInventoryService>();
            var devHubSnapshotService =
            new DevHubSnapshotService(
                missionCollection,
                objectiveCollection,
                progressService,
                inventoryService);

            context.Services.Register(
                devHubSnapshotService);


            context.Services.Register(
                eventHistoryService);
            context.Services.Register(
                interactionService);
            context.Services.Register(
                progressService);
            context.Services.Register(
                saveService);
            context.Services.Register(
                progressSaveCoordinator);
            context.Services.Register<ISaveDiagnosticsService>(
                diagnostics);


            return Task.CompletedTask;
        }
    }
}