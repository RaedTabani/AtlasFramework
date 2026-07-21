using System;
using DeviGames.Atlas.Core.Services;

namespace DeviGames.Atlas.Core.Triggers.Models
{
    public sealed class TriggerBuildContext
    {
        public ServiceRegistry Services { get; }

        public TriggerBuildContext(
            ServiceRegistry services)
        {
            Services =
                services
                ?? throw new ArgumentNullException(
                    nameof(services));
        }
    }
}