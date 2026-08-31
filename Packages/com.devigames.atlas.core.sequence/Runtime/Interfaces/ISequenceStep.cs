namespace DeviGames.Atlas.Core.Sequence.Interfaces
{
    public interface ISequenceStep
    {
        bool IsCompleted { get; }

        void Enter();
    }
}