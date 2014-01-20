namespace Humanizer
{
    class VerbEndsWithVowelAndY : ChangeTenseBase
    {
        public VerbEndsWithVowelAndY(string verb)
            : base(verb)
        {
        }

        public override bool Applies()
        {
            return Vowels.Contains(SecondLastChar) && LastChar == 'y';
        }

        public override string Apply()
        {
            return Verb + "ed";
        }
    }
}