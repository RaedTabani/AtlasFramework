using DeviGames.Atlas.Core.Content.Models;

namespace DeviGames.Atlas.Core.Content.Interfaces
{
    public interface IContentPackageConsumer
    {
        int Order { get; }

        void Install(
            ContentPackageData package);
    }
}