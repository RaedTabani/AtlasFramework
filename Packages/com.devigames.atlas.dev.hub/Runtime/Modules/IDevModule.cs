using DeviGames.Atlas.Dev.Hub.Context;

namespace DeviGames.Atlas.Dev.Hub.Modules
{
    /// <summary>
    /// Contract for a module displayed inside the Atlas Developer Hub.
    /// </summary>
    public interface IDevModule
    {
        /// <summary>
        /// Stable identifier used internally.
        /// Do not change after release.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// User-facing tab name.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Controls toolbar order. Lower values appear first.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Called when the user switches to this module.
        /// </summary>
        void OnActivate(DevHubContext context);

        /// <summary>
        /// Called when the user switches away from this module.
        /// </summary>
        void OnDeactivate(DevHubContext context);

        /// <summary>
        /// Draws the module UI.
        /// </summary>
        void Draw(DevHubContext context);
    }
}