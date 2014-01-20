using System.Collections.Generic;

namespace Humanizer
{
    public static class EnglishTenseExtensions
    {
        static EnglishTenseExtensions()
        {
            SetupDictionaries("Arise","Arose","Arisen");
            SetupDictionaries("Awake","Awoke","Awoken");
            SetupDictionaries("Be","Was","Been");
            SetupDictionaries("am","Was","Been");
            SetupDictionaries("are","Were","Been");
            SetupDictionaries("is","Was","Been");
            SetupDictionaries("Bear","Bore","Born" /* ToDo Borne */);
            SetupDictionaries("Beat","Beat","Beaten");
            SetupDictionaries("Become","Became","Become");
            SetupDictionaries("Begin","Began","Begun");
            SetupDictionaries("Bend","Bent","Bent");
            SetupDictionaries("Bet","Bet","Bet");
            SetupDictionaries("Bind","Bound","Bound");
            SetupDictionaries("Bid","Bid","Bid");
            SetupDictionaries("Bite","Bit","Bitten");
            SetupDictionaries("Bleed","Bled","Bled");
            SetupDictionaries("Blow","Blew","Blown");
            SetupDictionaries("Break","Broke","Broken");
            SetupDictionaries("Breed","Bred","Bred");
            SetupDictionaries("Bring","Brought","Brought");
            SetupDictionaries("Broadcast","Broadcast","Broadcast");
            SetupDictionaries("Build","Built","Built");
            //SetupDictionaries("Burn","Burnt /Burned","Burnt / Burned");
            SetupDictionaries("Burn","Burnt","Burnt");
            SetupDictionaries("Burst","Burst","Burst");
            SetupDictionaries("Buy","Bought","Bought");
            SetupDictionaries("Cast","Cast","Cast");
            SetupDictionaries("Can","Could","Could");
            SetupDictionaries("Catch","Caught","Caught");
            SetupDictionaries("Come","Came","Come");
            SetupDictionaries("Cost","Cost","Cost");
            SetupDictionaries("Cut","Cut","Cut");
            SetupDictionaries("Choose","Chose","Chosen");
            SetupDictionaries("Cling","Clung","Clung");
            SetupDictionaries("Creep","Crept","Crept");
            SetupDictionaries("Deal","Dealt","Dealt");
            SetupDictionaries("Dig","Dug","Dug");
            SetupDictionaries("Do","Did","Done");
            SetupDictionaries("Does","Did","Done");
            SetupDictionaries("Draw","Drew","Drawn");
            SetupDictionaries("Dream","Dreamt","Dreamt");
            //SetupDictionaries("Dream","Dreamt / Dreamed","Dreamt / Dreamed");
            SetupDictionaries("Drink","Drank","Drunk");
            SetupDictionaries("Drive","Drove","Driven");
            SetupDictionaries("Eat","Ate","Eaten");
            SetupDictionaries("Fall","Fell","Fallen");
            SetupDictionaries("Feed","Fed","Fed");
            SetupDictionaries("Feel","Felt","Felt");
            SetupDictionaries("Fight","Fought","Fought");
            SetupDictionaries("Find","Found","Found");
            SetupDictionaries("Flee","Fled","Fled");
            SetupDictionaries("Fly","Flew","Flown");
            SetupDictionaries("Forbid","Forbade","Forbidden");
            SetupDictionaries("Forget","Forgot","Forgotten");
            SetupDictionaries("Forgive","Forgave","Forgiven");
            SetupDictionaries("Freeze","Froze","Frozen");
            SetupDictionaries("Get","Got","Gotten");
            //SetupDictionaries("Get","Got","Got / Gotten");
            SetupDictionaries("Give","Gave","Given");
            SetupDictionaries("Go","Went","Gone");
            SetupDictionaries("Goes","Went","Gone");
            SetupDictionaries("Grow","Grew","Grown");
            SetupDictionaries("Grind","Ground","Ground");
            SetupDictionaries("Hang","Hung","Hung");
            SetupDictionaries("Have","Had","Had");
            SetupDictionaries("Hear","Heard","Heard");
            SetupDictionaries("Hide","Hid","Hidden");
            SetupDictionaries("Hit","Hit","Hit");
            SetupDictionaries("Hold","Held","Held");
            SetupDictionaries("Hurt","Hurt","Hurt");
            SetupDictionaries("Keep","Kept","Kept");
            SetupDictionaries("Know","Knew","Known");
            SetupDictionaries("Kneel","Knelt","Knelt");
            SetupDictionaries("Knit","Knit","Knit");
            SetupDictionaries("Lay","Laid","Laid");
            SetupDictionaries("Lead","Led","Led");
            SetupDictionaries("Lean","Leant","Leant");
            SetupDictionaries("Leap","Leapt","Leapt");
            SetupDictionaries("Learn","Learnt","Learnt");
            //SetupDictionaries("Learn","Learnt / Learned","Learnt / Learned");
            SetupDictionaries("Leave","Left","Left");
            SetupDictionaries("Lend","Lent","Lent");
            SetupDictionaries("Let","Let","Let");
            SetupDictionaries("Lie","Lay","Lain");
            SetupDictionaries("Light","Lit","Lit");
            SetupDictionaries("Lose","Lost","Lost");
            SetupDictionaries("Make","Made","Made");
            SetupDictionaries("Mean","Meant","Meant");
            SetupDictionaries("Meet","Met","Met");
            SetupDictionaries("Mistake","Mistook","Mistaken");
            SetupDictionaries("Overcome","Overcame","Overcome");
            SetupDictionaries("Pay","Paid","Paid");
            SetupDictionaries("Put","Put","Put");
            SetupDictionaries("Read","Read","Read");
            SetupDictionaries("Ride","Rode","Ridden");
            SetupDictionaries("Ring","Rang","Rung");
            SetupDictionaries("Rise","Rose","Risen");
            SetupDictionaries("Run","Ran","Run");
            SetupDictionaries("Say","Said","Said");
            SetupDictionaries("See","Saw","Seen");
            SetupDictionaries("Seek","Sought","Sought");
            SetupDictionaries("Sell","Sold","Sold");
            SetupDictionaries("Send","Sent","Sent");
            SetupDictionaries("Set","Set","Set");
            SetupDictionaries("Sew","Sewed","Sewed");
            //SetupDictionaries("Sew","Sewed","Sewed / Sewn");
            SetupDictionaries("Shake","Shook","Shaken");
            SetupDictionaries("Shear","Shore","Shorn");
            SetupDictionaries("Shine","Shone","Shone");
            SetupDictionaries("Shoot","Shot","Shot");
            SetupDictionaries("Show","Showed","Shown");
            SetupDictionaries("Shrink","Shrank","Shrunk");
            SetupDictionaries("Shut","Shut","Shut");
            SetupDictionaries("Sing","Sang","Sung");
            SetupDictionaries("Sink","Sank","Sunk");
            SetupDictionaries("Sit","Sat","Sat");
            SetupDictionaries("Sleep","Slept","Slept");
            SetupDictionaries("Slide","Slid","Slid");
            SetupDictionaries("Smell","Smelt","Smelt");
            SetupDictionaries("Sow","Sowed","Sowed");
            //SetupDictionaries("Sow","Sowed","Sowed / Sown");
            SetupDictionaries("Speak","Spoke","Spoken");
            SetupDictionaries("Speed","Sped","Sped");
            SetupDictionaries("Spell","Spelt","Spelt");
            SetupDictionaries("Spend","Spent","Spent");
            SetupDictionaries("Spill","Spilt","Spilt");
            //SetupDictionaries("Spill","Spilt / Spilled","Spilt / Spilled");
            SetupDictionaries("Spin","Spun","Spun");
            SetupDictionaries("Spit","Spat","Spat");
            SetupDictionaries("Split","Split","Split");
            SetupDictionaries("Spoil","Spoilt","Spoilt");
            //SetupDictionaries("Spoil","Spoilt / Spoiled","Spoilt / Spoiled");
            SetupDictionaries("Spread","Spread","Spread");
            SetupDictionaries("Spring","Sprang","Sprung");
            SetupDictionaries("Stand","Stood","Stood");
            SetupDictionaries("Steal","Stole","Stolen");
            SetupDictionaries("Stick","Stuck","Stuck");
            SetupDictionaries("Sting","Stung","Stung");
            SetupDictionaries("Stink","Stank","Stunk");
            SetupDictionaries("Stride","Strode","Stridden");
            SetupDictionaries("Strike","Struck","Struck");
            SetupDictionaries("Swear","Swore","Sworn");
            SetupDictionaries("Sweat","Sweat","Sweat");
            SetupDictionaries("Sweep","Swept","Swept");
            SetupDictionaries("Swell","Swelled","Swollen");
            SetupDictionaries("Swim","Swam","Swum");
            SetupDictionaries("Swing","Swung","Swung");
            SetupDictionaries("Take","Took","Taken");
            SetupDictionaries("Teach","Taught","Taught");
            SetupDictionaries("Tear","Tore","Torn");
            SetupDictionaries("Tell","Told","Told");
            SetupDictionaries("Think","Thought","Thought");
            SetupDictionaries("Throw","Threw","Thrown");
            SetupDictionaries("Thrust","Thrust","Thrust");
            SetupDictionaries("Tread","Trod","Trodden");
            SetupDictionaries("Understand","Understood","Understood");
            SetupDictionaries("Undergo","Underwent","Undergone");
            SetupDictionaries("Undertake","Undertook","Undertaken");
            SetupDictionaries("Wake","Woke","Woken");
            SetupDictionaries("Wear","Wore","Worn");
            SetupDictionaries("Weave","Wove","Woven");
            SetupDictionaries("Weep","Wept","Wept");
            SetupDictionaries("Wet","Wet","Wet");
            SetupDictionaries("Win","Won","Won");
            SetupDictionaries("Wind","Wound","Wound");
            SetupDictionaries("Withdraw","Withdrew","Withdrawn");
            SetupDictionaries("Wring","Wrung","Wrung");
            SetupDictionaries("Write","Wrote","Written");
        }

        static readonly Dictionary<string, string> PresentToSimplePast = new Dictionary<string, string>();
        static readonly Dictionary<string, string> PresentToPastParticple = new Dictionary<string, string>();
        private static void SetupDictionaries(string present, string simplePast, string pastParticiple)
        {
            PresentToSimplePast.Add(present, simplePast);
            PresentToPastParticple.Add(present, pastParticiple);
        }

        //http://www.scribd.com/doc/5810860/Spelling-Rules-for-Regular-Past-Tense-Verbs
        private static string AppendEd(string present)
        {
            var rules = new List<IChangeTense>
            {
                new VerbEndsWithVowelAndY(present), 
                new VerbEndsWithVowelAndConsonant(present),
                new VerbEndsWithE(present),
                new VerbEndsWithY(present)
            };

            foreach (var rule in rules)
            {
                if (rule.Applies())
                    return rule.Apply();
            }

            // Catch all: + ed
            return present + "ed";
        }

        /// <summary>
        /// Converts a present verb to simple past tense
        /// </summary>
        /// <param name="present"></param>
        /// <returns></returns>
        public static string ToPast(this string present)
        {
            if (PresentToSimplePast.ContainsKey(present))
                return PresentToSimplePast[present];

            return AppendEd(present);
        }

        /// <summary>
        /// Converts a present verb to past participle tense
        /// </summary>
        /// <param name="present"></param>
        /// <returns></returns>
        public static string ToPastParticiple(this string present)
        {
            if (PresentToPastParticple.ContainsKey(present))
                return PresentToPastParticple[present];

            return AppendEd(present);
        }
    }
}
