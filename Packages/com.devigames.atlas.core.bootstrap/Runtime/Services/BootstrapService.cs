using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Bootstrap.Events;
using DeviGames.Atlas.Core.Bootstrap.Steps;
using DeviGames.Atlas.Core.Events;

namespace DeviGames.Atlas.Core.Bootstrap.Services
{
    public sealed class Bootstrapper
    {
        private readonly List<IBootstrapStep> _steps = new();

        public bool IsRunning { get; private set; }
        public bool IsCompleted { get; private set; }

        public void AddStep(IBootstrapStep step)
        {
            if (step == null)
                throw new ArgumentNullException(nameof(step));

            _steps.Add(step);
        }

        public Bootstrapper AddStep<T>() where T : IBootstrapStep, new()
        {
            var step = new T();
            _steps.Add(step);
            return this; // Enables chaining
        }

        public async Task RunAsync()
        {
            if (IsRunning)
                return;

            IsRunning = true;
            IsCompleted = false;

            EventBus.Publish(new BootstrapStartedEvent());

            try
            {
                foreach (IBootstrapStep step in _steps)
                {
                    await step.ExecuteAsync();
                }

                IsCompleted = true;
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