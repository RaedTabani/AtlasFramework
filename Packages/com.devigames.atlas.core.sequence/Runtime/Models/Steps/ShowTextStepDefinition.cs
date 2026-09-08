namespace DeviGames.Atlas.Core.Sequence.Models
{
    public sealed class ShowTextStepDefinition :
        SequenceStepDefinition
    {
        public const string StepType =
            "show-text";

        public string Text;

        public ShowTextStepDefinition(
            string text)
            : base(
                StepType)
        {
            Text =
                text;
        }
    }
}