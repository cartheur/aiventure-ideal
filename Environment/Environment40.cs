using Cartheur.Ideal.Mooc.Coupling;

namespace Cartheur.Ideal.Mooc.Environment
{
    /// <summary>
    /// Results in R2 when the current experience equals the previous experience and differs from the penultimate experience and in R1 otherwise e1->r1 e1->r2 e2->r1 e2->r2 etc. 
    /// </summary>
    public static class Environment40
    {
        static Experiment _penultimateExperience;
        public static bool Stored { get; set; }
        public static Experiment PenultimateExperience { get; set; }
        public static void SetPenultimateExperience(Experiment penultimateExperience)
        {
            _penultimateExperience = penultimateExperience;
        }
        public static Experiment GetPenultimateExperience()
        {
            if (Stored)
                return PenultimateExperience;
            else
                return null;
        }
    }
}
