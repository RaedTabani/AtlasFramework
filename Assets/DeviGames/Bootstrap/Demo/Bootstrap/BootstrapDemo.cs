
using System;
using System.Threading.Tasks;
using DeviGames.Atlas.Core.Bootstrap.Events;
using DeviGames.Atlas.Core.Bootstrap.Services;
using DeviGames.Atlas.Core.Bootstrap.Steps;
using DeviGames.Atlas.Core.Events;
using UnityEngine;

public sealed class BootstrapDemo : MonoBehaviour
{
    private Bootstrapper _bootstrapper;

    private void OnEnable()
    {
        EventBus.Subscribe<BootstrapStartedEvent>(OnBootstrapStarted);
        EventBus.Subscribe<BootstrapCompletedEvent>(OnBootstrapCompleted);
        EventBus.Subscribe<BootstrapFailedEvent>(OnBootstrapFailed);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<BootstrapStartedEvent>(OnBootstrapStarted);
        EventBus.Unsubscribe<BootstrapCompletedEvent>(OnBootstrapCompleted);
        EventBus.Unsubscribe<BootstrapFailedEvent>(OnBootstrapFailed);
    }

    private async void Start()
    {
        _bootstrapper = new Bootstrapper();

        _bootstrapper.AddStep(new DelayBootstrapStep("Load Settings", 500));
        _bootstrapper.AddStep(new DelayBootstrapStep("Load Save Data", 500));
        _bootstrapper.AddStep(new DelayBootstrapStep("Check Content", 500));

        await RunBootstrapAsync();
    }

    private async Task RunBootstrapAsync()
    {
        try
        {
            await _bootstrapper.RunAsync();
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
        }
    }

    private void OnBootstrapStarted(BootstrapStartedEvent e)
    {
        Debug.Log("Bootstrap started.");
    }

    private void OnBootstrapCompleted(BootstrapCompletedEvent e)
    {
        Debug.Log("Bootstrap completed.");
    }

    private void OnBootstrapFailed(BootstrapFailedEvent e)
    {
        Debug.LogError($"Bootstrap failed: {e.Exception.Message}");
    }
}