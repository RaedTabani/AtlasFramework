using System.Threading.Tasks;

namespace DeviGames.Atlas.Core.Bootstrap.Steps
{
    public interface IBootstrapStep
    {
        string Name { get; }

        Task ExecuteAsync();
    }
}