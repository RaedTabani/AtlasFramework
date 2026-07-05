using System;
using System.Collections.Generic;
using DeviGames.Atlas.Dev.Console.Interfaces;

namespace DeviGames.Atlas.Dev.Console.Services
{
    public sealed class DevCommandRegistry
    {
        private readonly Dictionary<string, IDevCommand> _commands = new();

        public IReadOnlyDictionary<string, IDevCommand> Commands => _commands;

        public void Register(IDevCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            _commands[command.Name] = command;
        }

        public bool TryExecute(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            string[] parts = input.Split(' ');
            string commandName = parts[0];

            if (!_commands.TryGetValue(commandName, out IDevCommand command))
                return false;

            string[] args = new string[parts.Length - 1];

            for (int i = 1; i < parts.Length; i++)
                args[i - 1] = parts[i];

            command.Execute(args);
            return true;
        }
    }
}