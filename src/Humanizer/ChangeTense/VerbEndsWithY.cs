namespace Humanizer
{
    class VerbEndsWithY : ChangeTenseBase
    {
        public VerbEndsWithY(string verb)
            : base(verb)
        {
        }

        public override bool Applies()
        {
            return LastChar == 'y';
        }

        public override string Apply()
        {
            return Verb.TrimEnd('y') + "ied";
        }
    }
}