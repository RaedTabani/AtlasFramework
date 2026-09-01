namespace DeviGames.Atlas.Core.Sequence.Models
{
    public sealed class WaitForContinueStepDefinition :
        SequenceStepDefinition
    {
        public const string StepType =
            "wait-for-continue";

        public WaitForContinueStepDefinition()
            : base(
                StepType)
        {
        }
    }
}