using System.IO;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Bootstrap.Context;
using DeviGames.Atlas.Core.Bootstrap.Steps;
using DeviGames.Atlas.Core.Interaction.Services;
using DeviGames.Atlas.Core.Missions.Services;
using DeviGames.Atlas.Core.Objectives.Services;
using DeviGames.Atlas.Core.Progress.Services;
using DeviGames.Atlas.Core.Save.Services;
using DeviGames.Atlas.Core.Save.Storage;
using DeviGames.Atlas.Gameplay.Inventory.Services;
using DeviGames.Atlas.Gameplay.Objectives.Services;
using DeviGames.Atlas.Core.Diagnostics.Services;
using UnityEngine;

namespace DeviGames.Playground.Bootstrap
{
    public sealed class RegisterPlaygroundServicesStep : IBootstrapStep
    {
        public string Name => "Register Playground Services";

        public Task ExecuteAsync(BootstrapContext context)
        {
            string savePath = Path.Combine(
                UnityEngine.Application.persistentDataPath,
                "DeviGames",
                "Playground",
                "Saves");

            var eventHistoryService = new EventHistoryService(250);
            var interactionService = new InteractionService();
            var inventoryService = new InventoryService();
            var objectiveService = new ObjectiveService();
            var progressService = new MissionProgressService();

            var saveService = new SaveService(
                new JsonFileSaveStorage(savePath));

            var objectiveAdapter =
                new GameplayObjectiveAdapter(objectiveService);

            var missionService =
                new MissionService(objectiveService);

            var progressSaveCoordinator =
                new ProgressSaveCoordinator(
                    progressService,
                    saveService);


            context.Services.Register(eventHistoryService);
            context.Services.Register(interactionService);
            context.Services.Register(inventoryService);
            context.Services.Register(objectiveService);
            context.Services.Register(progressService);
            context.Services.Register(saveService);
            context.Services.Register(objectiveAdapter);
            context.Services.Register(missionService);
            context.Services.Register(progressSaveCoordinator);

            return Task.CompletedTask;
        }
    }
}