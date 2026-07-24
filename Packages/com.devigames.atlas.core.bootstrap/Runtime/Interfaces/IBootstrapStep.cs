using System.Threading.Tasks;
using DeviGames.Atlas.Core.Bootstrap.Models;
namespace DeviGames.Atlas.Core.Bootstrap.Interfaces
{
    public interface IBootstrapStep
    {
        string Name { get; }

        Task ExecuteAsync(BootstrapContext context);
    }
}