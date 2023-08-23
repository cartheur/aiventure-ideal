using Cartheur.Ideal.Mooc.Coupling;

namespace Cartheur.Ideal.Mooc.Environment
{
    /// <summary>
    /// The agent must alternate experiences e1 and e2 every third cycle to get one r2 result the third time:
    /// e1->r1 e1->r1 e1->r2 e2->r1 e2->r1 e2->r2 etc.
    /// </summary>
    public static class Environment41
    {
        /**
	 * Environment041
	 * 
	 */
        static Experiment _antepenultimateExperience;
        public static bool Stored { get; set;}
        public static Experiment AntepenultimateExperience { get; set; }
        public static void SetAntePenultimateExperience(Experiment antepenultimateExperience)
        {
            _antepenultimateExperience = antepenultimateExperience;
        }
        public static Experiment GetAntePenultimateExperience()
        {
            if (Stored)
            return AntepenultimateExperience;
            else
                return null;
        }
    }
}
