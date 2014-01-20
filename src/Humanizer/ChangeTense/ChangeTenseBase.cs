using System.Collections.Generic;

namespace Humanizer
{
    abstract class ChangeTenseBase : IChangeTense
    {
        protected string Verb { get; private set; }
        protected static readonly List<char> Vowels = new List<char> { 'a', 'e', 'i', 'o', 'u' };

        protected ChangeTenseBase(string verb)
        {
            Verb = verb;
        }

        protected char LastChar { get { return Verb[Verb.Length - 1]; } }
        protected char SecondLastChar { get { return Verb[Verb.Length - 2]; } }

        public abstract bool Applies();
        public abstract string Apply();
    }

    class CatchAll : ChangeTenseBase
    {
        public CatchAll(string verb) : base(verb)
        {
        }

        public override bool Applies()
        {
            return true;
        }

        public override string Apply()
        {
            return Verb + "ed";
        }
    }
}