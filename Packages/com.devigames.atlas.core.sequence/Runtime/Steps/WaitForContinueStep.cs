using DeviGames.Atlas.Core.Sequence.Interfaces;

namespace DeviGames.Atlas.Core.Sequence.Steps
{
    public sealed class WaitForContinueStep :
        ISequenceStep,
        IContinuableSequenceStep
    {
        public bool IsCompleted { get; private set; }

        public void Enter()
        {
        }

        public bool Continue()
        {
            if (IsCompleted)
            {
                return false;
            }

            IsCompleted =
                true;

            return true;
        }
    }
}