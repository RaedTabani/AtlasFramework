using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Services;
using DeviGames.Atlas.Core.Bootstrap.Context;
using DeviGames.Atlas.Core.Bootstrap.Events;
using DeviGames.Atlas.Core.Bootstrap.Steps;
using DeviGames.Atlas.Core.Events;


namespace DeviGames.Atlas.Core.Bootstrap.Services
{
    public sealed class BootstrapService
    {
        private readonly List<IBootstrapStep> _steps = new();

        public bool IsRunning { get; private set; }
        public bool IsCompleted { get; private set; }

        public ServiceRegistry ServiceRegistry { get; private set; }

        public void AddStep(IBootstrapStep step)
        {
            if (step == null)
                throw new ArgumentNullException(nameof(step));

            _steps.Add(step);
        }

        public BootstrapService AddStep<T>() where T : IBootstrapStep, new()
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

            ServiceRegistry = new ServiceRegistry();
            BootstrapContext context = new BootstrapContext(ServiceRegistry);

            EventBus.Publish(new BootstrapStartedEvent());

            try
            {
                foreach (IBootstrapStep step in _steps)
                {
                    await step.ExecuteAsync(context);
                }

                IsCompleted = true;
                DeviGames.Atlas.Core.Services.Services.SetRegistry(ServiceRegistry);
                EventBus.Publish(new BootstrapCompletedEvent());
            }
            catch (Exception exception)
            {
                EventBus.Publish(new BootstrapFailedEvent(exception));
                throw;
            }
            finally
            {
                IsRunning = false;
            }
        }
    }
}