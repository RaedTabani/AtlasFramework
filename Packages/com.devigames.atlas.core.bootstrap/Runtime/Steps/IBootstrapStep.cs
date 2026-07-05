using System.Threading.Tasks;
using DeviGames.Atlas.Core.Bootstrap.Context;
namespace DeviGames.Atlas.Core.Bootstrap.Steps
{
    public interface IBootstrapStep
    {
        string Name { get; }

        Task ExecuteAsync(BootstrapContext context);
    }
}