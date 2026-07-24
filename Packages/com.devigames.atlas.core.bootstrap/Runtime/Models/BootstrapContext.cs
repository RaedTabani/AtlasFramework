using DeviGames.Atlas.Core.Services;

namespace DeviGames.Atlas.Core.Bootstrap.Models
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