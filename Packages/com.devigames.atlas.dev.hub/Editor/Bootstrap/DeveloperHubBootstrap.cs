using DeviGames.Atlas.Dev.Hub.Editor.Modules;
using DeviGames.Atlas.Dev.Hub.Modules;

namespace DeviGames.Atlas.Dev.Hub.Editor.Bootstrap
{
    internal static class DeveloperHubBootstrap
    {
        public static void RegisterModules(
            DevModuleRegistry registry)
        {
            registry.Register(
                new RuntimeModule());

            registry.Register(
                new GameplayModule());

            registry.Register(
                new EventsModule());

            registry.Register(
                new SaveModule());
        }
    }
}