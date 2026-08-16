using System.Collections.Generic;

namespace DeviGames.Atlas.Core.Content.Validation
{
    public sealed class ContentValidationResult
    {
        private readonly List<string> _errors =
            new();

        public IReadOnlyList<string> Errors =>
            _errors;

        public bool IsValid =>
            _errors.Count == 0;

        public void AddError(
            string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                _errors.Add(error);
            }
        }
    }
}