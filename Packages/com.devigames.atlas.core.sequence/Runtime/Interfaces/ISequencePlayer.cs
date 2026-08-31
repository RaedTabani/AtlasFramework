using DeviGames.Atlas.Core.Sequence.Services;

namespace DeviGames.Atlas.Core.Sequence.Interfaces
{
    public interface ISequencePlayer
    {
        SequenceRuntime ActiveSequence { get; }

        bool IsPlaying { get; }

        bool Play(
            SequenceRuntime sequence);

        bool Complete();
        bool Advance();
    }
}