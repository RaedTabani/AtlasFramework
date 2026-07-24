using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Bootstrap.Models;
using DeviGames.Atlas.Core.Bootstrap.Events;
using DeviGames.Atlas.Core.Bootstrap.Interfaces;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Core.Services;

namespace DeviGames.Atlas.Core.Bootstrap.Services
{
    public sealed class BootstrapService
    {
        private readonly List<IBootstrapStep> _steps = new();

        public bool IsRunning { get; private set; }
        public bool IsCompleted { get; private set; }

        public BootstrapService AddStep(IBootstrapStep step)
        {
            if (step == null)
                throw new ArgumentNullException(nameof(step));

            _steps.Add(step);
            return this;
        }

        public BootstrapService AddStep<T>()
            where T : IBootstrapStep, new()
        {
            _steps.Add(new T());
            return this;
        }

        public async Task RunAsync()
        {
            if (IsRunning)
                return;

            IsRunning = true;
            IsCompleted = false;

            var registry = new ServiceRegistry();
            var context = new BootstrapContext(registry);

            EventBus.Publish(new BootstrapStartedEvent());

            try
            {
                foreach (IBootstrapStep step in _steps)
                {
                    await step.ExecuteAsync(context);
                }

                await registry.InitializeAsync();

                DeviGames.Atlas.Core.Services.Services.SetRegistry(registry);

                IsCompleted = true;

                EventBus.Publish(new BootstrapCompletedEvent());
            }
            catch (Exception exception)
            {
                registry.Shutdown();

                EventBus.Publish(
                    new BootstrapFailedEvent(exception));

                throw;
            }
            finally
            {
                IsRunning = false;
            }
        }
    }
}