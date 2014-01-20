namespace Humanizer
{
    class VerbEndsWithE : ChangeTenseBase
    {
        public VerbEndsWithE(string verb)
            : base(verb)
        {
        }

        public override bool Applies()
        {
            return LastChar == 'e';
        }

        public override string Apply()
        {
            return Verb + 'd';
        }
    }
}