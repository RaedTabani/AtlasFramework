using DeviGames.Atlas.Core.Services;

namespace DeviGames.Atlas.Core.Triggers.Context
{
    public sealed class TriggerContext
    {
        public ServiceRegistry Services { get; }

        public TriggerContext(
            ServiceRegistry services)
        {
            Services = services;
        }
    }
}