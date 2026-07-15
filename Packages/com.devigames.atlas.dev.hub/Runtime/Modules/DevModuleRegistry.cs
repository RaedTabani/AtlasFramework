using System;
using System.Collections.Generic;

namespace DeviGames.Atlas.Dev.Hub.Modules
{
    /// <summary>
    /// Stores Developer Hub modules and maintains their display order.
    /// </summary>
    public sealed class DevModuleRegistry
    {
        private readonly List<IDevModule> _modules = new();

        public IReadOnlyList<IDevModule> Modules => _modules;

        public int Count => _modules.Count;

        public void Register(IDevModule module)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));

            if (string.IsNullOrWhiteSpace(module.Id))
            {
                throw new ArgumentException(
                    "Developer Hub module id cannot be empty.",
                    nameof(module));
            }

            if (_modules.Exists(
                    existing => existing.Id == module.Id))
            {
                throw new InvalidOperationException(
                    $"Developer Hub module '{module.Id}' " +
                    "is already registered.");
            }

            _modules.Add(module);

            _modules.Sort(
                (left, right) =>
                {
                    int orderComparison =
                        left.Order.CompareTo(right.Order);

                    if (orderComparison != 0)
                        return orderComparison;

                    return string.Compare(
                        left.DisplayName,
                        right.DisplayName,
                        StringComparison.Ordinal);
                });
        }

        public bool Unregister(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            int index = _modules.FindIndex(
                module => module.Id == id);

            if (index < 0)
                return false;

            _modules.RemoveAt(index);
            return true;
        }

        public bool TryGet(
            string id,
            out IDevModule module)
        {
            module = _modules.Find(
                candidate => candidate.Id == id);

            return module != null;
        }

        public void Clear()
        {
            _modules.Clear();
        }
    }
}