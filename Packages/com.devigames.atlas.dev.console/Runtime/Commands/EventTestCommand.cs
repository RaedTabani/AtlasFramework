using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Dev.Console.Interfaces;
using UnityEngine;

namespace DeviGames.Atlas.Dev.Console.Commands
{
    public sealed class EventTestCommand : IDevCommand
    {
        public string Name => "event_test";
        public string Description => "Publishes a test dev console event.";

        public void Execute(string[] args)
        {
            EventBus.Publish(new DevConsoleTestEvent());
            Debug.Log("Dev console test event published.");
        }
    }

    public readonly struct DevConsoleTestEvent { }
}