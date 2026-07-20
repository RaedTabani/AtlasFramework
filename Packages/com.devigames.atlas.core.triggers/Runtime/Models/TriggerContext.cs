using DeviGames.Atlas.Core.Services.Interfaces;

namespace DeviGames.Atlas.Core.Triggers.Models
{
    public sealed class TriggerContext
    {
        public IServiceResolver Services { get; }

        public TriggerContext(
            IServiceResolver services)
        {
            Services = services;
        }
    }
}