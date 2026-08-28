using System;

using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.GameFlow.Interfaces;
using DeviGames.Atlas.Core.GameFlow.Services;
using DeviGames.Atlas.Core.Services;

namespace DeviGames.Atlas.Core.GameFlow.Installation
{
    public sealed class GameFlowInstaller :
        IAtlasInstaller
    {
        public void Install(
            AtlasInstallationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ServiceRegistry services =
                context.Services;

            var gameFlowService =
                new GameFlowService();

            services.Register<IGameFlowService>(
                gameFlowService);
        }
    }
}