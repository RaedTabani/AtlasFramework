using DeviGames.Atlas.Core.Services;

namespace DeviGames.Atlas.Core.Bootstrap.Context
{
    public sealed class BootstrapContext
    {
        public ServiceRegistry Services { get; }

        public BootstrapContext(ServiceRegistry services)
        {
            Services = services;
        }
    }
}