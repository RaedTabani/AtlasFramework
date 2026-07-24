using DeviGames.Atlas.Core.Bootstrap.Models;

namespace DeviGames.Atlas.Core.Bootstrap.Interfaces
{
    public interface IAtlasInstaller
    {
        void Install(
            AtlasInstallationContext context);
    }
}