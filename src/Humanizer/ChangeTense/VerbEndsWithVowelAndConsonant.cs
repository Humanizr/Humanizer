namespace Humanizer
{
    class VerbEndsWithVowelAndConsonant : ChangeTenseBase
    {
        public VerbEndsWithVowelAndConsonant(string verb)
            : base(verb)
        {
        }

        public override bool Applies()
        {
            return Vowels.Contains(SecondLastChar) && !Vowels.Contains(LastChar);
        }

        public override string Apply()
        {
            if (LastChar != 'x' && LastChar != 'w')
                return Verb + LastChar + "ed";

            return Verb + "ed";
        }
    }
}