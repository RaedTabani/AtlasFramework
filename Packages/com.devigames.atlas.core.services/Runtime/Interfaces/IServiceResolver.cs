namespace DeviGames.Atlas.Core.Services.Interfaces
{
    public interface IServiceResolver
    {
        T Resolve<T>();

        bool TryResolve<T>(
            out T service);
    }
}