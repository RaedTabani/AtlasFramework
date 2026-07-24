using System;
using DeviGames.Atlas.Core.Services;

namespace DeviGames.Atlas.Core.Bootstrap.Models
{
    public sealed class AtlasInstallationContext
    {
        public ServiceRegistry Services { get; }

        public AtlasInstallationContext(
            ServiceRegistry services)
        {
            Services =
                services
                ?? throw new ArgumentNullException(
                    nameof(services));
        }
    }
}